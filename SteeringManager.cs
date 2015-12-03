using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace com.kuxiong.battlemodule {

    [System.Serializable]
    public class SteeringMgr
    {
        public Vector3 steering;

        public BaseControl host;

        public bool debug = true;
 
        public float MAX_VELOCITY;


        /// The constructor
        public SteeringMgr(BaseControl host, float MAX_VELOCITY)
        {
            this.host = host;
            this.steering = Vector3.zero;
            this.MAX_VELOCITY = MAX_VELOCITY;
        }

        #region The public API (one method for each behavior)
        public void seek(Vector3 target) { steering += doSeek(target); }
        public void flee(Vector3 target) { steering += doFlee(target); }
        public void arrival(Vector3 target, float slowingRadius) { steering += doArrival(target, slowingRadius); }
        public void acc(Vector3 target, float initDistance) { steering += doAcc(target, initDistance); }

        public void collisionAvoidance(float MAX_SEE_AHEAD, float MAX_AVOID_FORCE, List<BaseControl> obstacles, float radius)
        {
            steering += doCollisionAvoidance(MAX_SEE_AHEAD, MAX_AVOID_FORCE, obstacles, radius);
        }
        public void queue(List<BaseControl> boids, float MAX_QUEUE_AHEAD, float MAX_QUEUE_RADIUS, float SEPARATION_RADIUS, float MAX_SEPARATION)
        {
            steering += doQueue(boids, MAX_QUEUE_AHEAD, MAX_QUEUE_RADIUS, SEPARATION_RADIUS, MAX_SEPARATION);
        }
        public void
            flocking(List<NormalControl> boids, 
                out float cohesionAdd)
        {            
            //Vector3 alignment;
            Vector3 cohesion;
            Vector3 separation;
            doFlockingV2(boids, out cohesion, out separation, out cohesionAdd);
            steering += (cohesion + separation);
        }
        #endregion

        #region The internal API
        private Vector3 doSeek(Vector3 target)
        {
            //Debug.Log("difference " + (target - host.transform.position));

            Vector3 force = Vector3.zero;

            var desired_velocity = (target - host.transform.position).normalized * MAX_VELOCITY;

            force = desired_velocity - host.velocity;

            return force;
        }
        private Vector3 doFlee(Vector3 target)
        {
            return -doSeek(target);
        }
        /// The real implementation of seek (with arrival code included)
        private Vector3 doArrival(Vector3 target, float slowingRadius)
        {
            Vector3 force = Vector3.zero;

            var desired_velocity = target - host.transform.position;
            var distance = desired_velocity.magnitude;
            if (distance <= slowingRadius)
            {
                desired_velocity = desired_velocity.normalized * MAX_VELOCITY * (distance / slowingRadius);
            }
            else
            {
                desired_velocity = desired_velocity.normalized * MAX_VELOCITY;
            }

            force = desired_velocity - host.velocity;

            return force;
        }

        /// 加速运动
        public Vector3 doAcc(Vector3 target, float initDistance)
        {
            Vector3 force = Vector3.zero;

            var desired_velocity = target - host.transform.position;

            var distance = desired_velocity.sqrMagnitude;

            float addRate = Mathf.Abs((1.0f + (initDistance - distance) / initDistance)); Debug.Log("addRate " + addRate);

            desired_velocity = desired_velocity.normalized * MAX_VELOCITY;

            force = desired_velocity - host.velocity;

            return force;
        }

        private Vector3 doCollisionAvoidance(float MAX_SEE_AHEAD, float MAX_AVOID_FORCE, List<BaseControl> obstacles, float radius)
        {
            Vector3 ahead = host.transform.position + host.velocity.normalized * MAX_SEE_AHEAD;
            Vector3 ahead2 = host.transform.position + host.velocity.normalized * MAX_SEE_AHEAD * 0.5f;

            //Debug.DrawLine(host.transform.position, ahead, Color.red);

            BaseControl mostThreatening = findMostThreateningObstacle(ahead, ahead2, obstacles, radius);
            
            Vector3 avoidance = Vector3.zero;

            if (mostThreatening != null)
            {
                avoidance = (ahead - mostThreatening.transform.position).normalized * MAX_AVOID_FORCE;
            }

            return avoidance;
        }

        private Vector3 doQueue(List<BaseControl> boids, float MAX_QUEUE_AHEAD, float MAX_QUEUE_RADIUS, float SEPARATION_RADIUS, float MAX_SEPARATION)
        {

            //Using the steering force directly is dangerous. 
            //If queue() is the first behavior to be applied to a character,
            //the steering force will be "empty". 
            //As a consequence, queue() must be invoked after all other steering methods, 
            //so that it can access the complete and final steering force.

            Vector3 v = host.velocity;
            Vector3 brake = Vector3.zero;

            BaseControl neighbor = getNeighborAhead(boids, MAX_QUEUE_AHEAD, MAX_QUEUE_RADIUS);
           
            if (neighbor != null){

               

                // TODO: take action because neighbor is ahead
                //if (steering == Vector3.zero) UnityEngine.Debug.LogError("this?!!!!!");
                brake = steering * 0.8f;
                v *= -1.0f;
                brake += v;
                //brake += separation(host, boids, SEPARATION_RADIUS, MAX_SEPARATION);

                if (Vector3.SqrMagnitude(neighbor.transform.position - host.transform.position) <= (MAX_QUEUE_RADIUS * MAX_QUEUE_RADIUS))
                {
                    host.velocity*= 0.3f;
                }
            }

            return brake;
        }

        private void doFlocking(List<BaseControl> boids,  float radius, out Vector3 alignment, out Vector3 cohesion, out Vector3 separation)
        {
            alignment = Vector3.zero;
            cohesion = Vector3.zero;
            separation = Vector3.zero;

            int neighborCount = 0;

            // Loop through each agent to determine the separation
            for (int i = 0; i < boids.Count; ++i)
            {
                // The agent can't compare against itself
                if (host != boids[i])
                {
                    // Only determine the parameters if the other agent is its neighbor
                    if (Vector3.SqrMagnitude(boids[i].transform.position - host.transform.position) < (radius * radius))
                    {
                        // This agent is the neighbor of the original agent so add the separation
                        alignment += boids[i].velocity;
                        cohesion += boids[i].transform.position;
                        separation += boids[i].transform.position - host.transform.position;

                        neighborCount++;
                    }
                }
            }

            // Don't move if there are no neighbors
            if (neighborCount == 0) return;

            alignment = (alignment / neighborCount).normalized;
            cohesion = ((cohesion / neighborCount) - host.transform.position).normalized;
            separation = ((separation / neighborCount) * -1).normalized;
        }


        public void doFlockingV2(
            List<NormalControl> boids, out Vector3 cohesion, out Vector3 speration, out float cohesionAdd)
        {
            cohesion = Vector3.zero;
            speration = Vector3.zero;

            int tooaway_neighbourCount = 0;
            int tooclose_neighborCount = 0;
           

            NormalControl self = host as NormalControl;

            foreach (NormalControl item in boids)
            {
                float cohesionThreshold = self.solider_Meta_Info.speration_radius + self.solider_Meta_Info.cohesion_radius;
                float sperateThreshold = self.solider_Meta_Info.speration_radius + item.solider_Meta_Info.speration_radius;

                ///离自己太远的
                //self radius + tolerate radius
                if (item != host 
                    && Vector3.SqrMagnitude(item.transform.position - host.transform.position) > (cohesionThreshold * cohesionThreshold))
                {
                    cohesion += item.transform.position;
                    tooaway_neighbourCount++;
                }

                //self radius + neighbour radius
                if (item != host
                        && Vector3.SqrMagnitude(item.transform.position - host.transform.position) < (sperateThreshold * sperateThreshold))
                {
                    speration += item.transform.position - host.transform.position;

                    tooclose_neighborCount++;
                }
            }

            if (tooclose_neighborCount != 0)
            {
                speration = ((speration / tooclose_neighborCount) * -1.0f).normalized;
            }
            else
            {
                speration = Vector3.zero;
            }


            ///全都远离你？
            if (tooaway_neighbourCount > (boids.Count / 2))
            {
                cohesionAdd = 0.1f;
                cohesion = ((cohesion / tooaway_neighbourCount) - host.transform.position).normalized * (1 + cohesionAdd);
            }
            else
            {
                cohesionAdd = 0f;
                cohesion = Vector3.zero;
            }
        }
        #endregion


        /// Reset the internal steering force.
        public void reset()
        {
            steering = Vector3.zero;
        }
        
        /// The update method. 
        /// Should be called after all behaviors have been invoked
        public void update()
        {
            //steering = nothing(); // the null vector, meaning "zero force magnitude"
            //steering = steering + seek(); // assuming the character is seeking something
            //steering = steering + collisionAvoidance();

            //steering = truncate(steering, MAX_FORCE);
            //steering /= host.mass;

            host.velocity = truncate(host.velocity + steering, MAX_VELOCITY);

            host.transform.position += host.velocity * Time.deltaTime;
           
        }

        public void update(CallbackWithArg moveCallBack, params object[] args)
        {
            host.velocity = truncate(host.velocity + steering, MAX_VELOCITY);
            host.transform.position += host.velocity * Time.deltaTime;

            if (moveCallBack != null) moveCallBack(args);
        }



        public void stop()
        {
            host.velocity = Vector3.zero;

            steering = Vector3.zero;
        }

        #region Util
        private BaseControl findMostThreateningObstacle(Vector3 ahead, Vector3 ahead2, List<BaseControl> obstacles, float radius)
        {
            BaseControl mostThreatening = null;

            foreach (BaseControl curObstacle in obstacles)
            {
                if (curObstacle == host) continue;

                bool collision = lineIntersectsCircle(ahead, ahead2, host.transform.position, curObstacle.transform.position, radius);

                if (collision && (mostThreatening == null || Vector3.SqrMagnitude(host.transform.position - curObstacle.transform.position) < Vector3.SqrMagnitude(host.transform.position - mostThreatening.transform.position)))
                {
                    mostThreatening = curObstacle;
                }
            }

            return mostThreatening;
        }
        private bool lineIntersectsCircle(Vector3 ahead, Vector3 ahead2, Vector3 curPos, Vector3 center, float radius)
        {
            #region legacy
            //Debug.Log(" Vector3.Distance(ahead, center) " + Vector3.Distance(ahead, center));
            //Debug.Log(" Vector3.Distance(ahead2, center) " + Vector3.Distance(ahead2, center));
            //Debug.Log(" Vector3.Distance(curPos, center) " + Vector3.Distance(curPos, center));
            //return Vector3.Distance(ahead, center) < radius || Vector3.Distance(ahead2, center) < radius || Vector3.Distance(curPos, center) < radius;
            #endregion

            return Vector3.SqrMagnitude(ahead - center) < (radius * radius) || Vector3.SqrMagnitude(ahead2 - center) < (radius * radius) || Vector3.SqrMagnitude(curPos - center) < (radius * radius);
        }
        private Vector3 truncate(Vector3 vector, float max)
        {
            float rate = vector.magnitude / max;
            return rate <= 1.0f ? vector : vector.normalized * max;  
        }

        private BaseControl getNeighborAhead(List<BaseControl> boids, float MAX_QUEUE_AHEAD, float MAX_QUEUE_RADIUS)
        {
            BaseControl ret  = null;

            Vector3 qa = host.velocity.normalized * MAX_QUEUE_AHEAD;

            Vector3 ahead = host.transform.position + qa;

           

            foreach (BaseControl item in boids)
            {
                float d = Vector3.SqrMagnitude(item.transform.position - ahead);

                if (item != host && d <= (MAX_QUEUE_RADIUS * MAX_QUEUE_RADIUS))
                {
                    ret = item;
                    break;
                }
            }

            return ret;
        }

        private Vector3 separation(
            BaseControl curAgent, 
                List<BaseControl> boids, 
                    float SEPARATION_RADIUS, 
                        float MAX_SEPARATION)
        {
            var separation = Vector3.zero;

            int neighborCount = 0;

            //var agentTransform = agents[curAgent].transform;

            // Loop through each agent to determine the separation
            for (int i = 0; i < boids.Count; ++i)
            {
                // The agent can't compare against itself
                if (curAgent != boids[i])
                {
                    // Only determine the parameters if the other agent is its neighbor
                    if (Vector3.SqrMagnitude(boids[i].transform.position - curAgent.transform.position) < (SEPARATION_RADIUS * SEPARATION_RADIUS))
                    {
                        // This agent is the neighbor of the original agent so add the separation
                        separation += boids[i].transform.position - curAgent.transform.position;

                        neighborCount++;
                    }
                }
            }

            // Don't move if there are no neighbors
            if (neighborCount == 0)
            {
                return Vector3.zero;
            }

            //Debug.Log(curAgent.name + " this ? " + neighborCount);

            // Normalize the value
            return ((separation / neighborCount) * -1).normalized * (MAX_SEPARATION);
        }




        #endregion
    }


}
