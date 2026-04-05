using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterElevator
{
    class ElevatorExitTrigger : MonoBehaviour
    {
        private Elevator elevator;
        private bool firstTime;
        private void Start()
        {
            firstTime = false;
        }
        public void SetElevator(Elevator elevator)
        {
            this.elevator = elevator;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && other.isTrigger)
            {
                if (!firstTime)
                    firstTime = true;
                else
                    elevator.ButtonPressed();

            }
        }
    }

}
