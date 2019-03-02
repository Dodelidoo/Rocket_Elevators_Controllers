using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
namespace Rocket_Elevators_Corporate_Controller
{
    class Program
    // Dodelidoo is the name of my Controller
    // all test begin here
    {
        static void Main(string[] args)
        {
            ElevatorController Dodelidoo1 = new ElevatorController(85, 4, 5);// battery properties
            Console.WriteLine("Starting initial Battery : Vrooom Vrooom Baby!! ");
            Console.WriteLine("Column number is : " + Dodelidoo1.nbColumns);
            Console.WriteLine("Battery name is :  " + Dodelidoo1);

            //Elevator origins
            Dodelidoo1.Battery1.columnList[1].elevatorList[0].FloorNumber = 1;
            Dodelidoo1.Battery1.columnList[1].elevatorList[1].FloorNumber = 23;
            Dodelidoo1.Battery1.columnList[1].elevatorList[2].FloorNumber = 33;
            Dodelidoo1.Battery1.columnList[1].elevatorList[3].FloorNumber = 40;
            Dodelidoo1.Battery1.columnList[1].elevatorList[4].FloorNumber = 42;

            //Find from floor and find from level
            //need to assign a specific elevator to request, not the closest...

            // Direction command / requested floors
            Dodelidoo1.AssignElevator(24);
            Dodelidoo1.AssignElevator(28);
            Dodelidoo1.RequestElevator(33,1);
            Dodelidoo1.AssignElevator(24);
            Dodelidoo1.RequestElevator(40, 1);
            Dodelidoo1.AssignElevator(36);

        
          //  Console.ReadLine();
        }
    }
    //=====================================Elevator Controller==================================
    class ElevatorController
    {
        public int requestedFloor;
        public int BuildingFloorNumber;
        public int nbElevatorsByColumns;
        public int nbColumns;
        public Battery Battery1;
        public ElevatorController(int BuildingFloorNumber, int nbColumns, int nbElevatorsByColumns)
        {
            this.BuildingFloorNumber = BuildingFloorNumber;
            this.nbColumns = nbColumns;
            this.Battery1 = new Battery(BuildingFloorNumber, nbColumns, nbElevatorsByColumns);
        }

        //elevator taking request of people with
        //requested floor number and number of floors

        //send the nearest elevator after its send
        // to a floor list with requested floor number
        //they send the nearest elevator to people

        public void RequestElevator(int FloorNumber, int requestedFloorNumber)
        {
            Console.WriteLine("someone requested an elevator at floor: " + FloorNumber);
            var column1 = Battery1.FindColumn(FloorNumber);
            var nearestElevator = column1.FindNearestElevator(FloorNumber);
            Console.WriteLine("CallButtonLightOn");
            Console.WriteLine("Returning " + nearestElevator.elevatorName + " of Column number " + Battery1.nbColumns);

            nearestElevator.AddFloorToList(requestedFloorNumber);
            nearestElevator.MoveNext();
        }
        // its gonna assign the best elevator depending where the user is moving to
        public void AssignElevator(int requestedFloorNumber)

        {
            Console.WriteLine("Requested Floor at : " + requestedFloorNumber);
            var column1 = Battery1.FindColumn(requestedFloorNumber);

            var nearestElevator = column1.FindNearestElevator(1);

            

            if (nearestElevator.FloorNumber != 1){
                nearestElevator.AddFloorToList(1);
                nearestElevator.MoveNext();
            } else {
                nearestElevator.OpeningAndClosingDoors();
            }

            nearestElevator.AddFloorToList(requestedFloorNumber);
            nearestElevator.MoveNext();
        }
    }
    //====================================== Battery =========================
    class Battery
    {
        //battery operate column and screen display panel
        //for people asking for elevator 

        public int nbFloors;
        public int nbColumns;
        public int nbElevatorsByColumns;
        public List<Column> columnList;
        public Battery(int nbFloors, int nbColumns, int nbElevatorsByColumns)
        {
            this.nbFloors = nbFloors;
            this.nbColumns = nbColumns;
            this.nbElevatorsByColumns = nbElevatorsByColumns;
            columnList = new List<Column>();
            this.CreateColumns();

        }
        //  Where columns is created      
        public void CreateColumns()
        {
            for (int i = 0; i < this.nbColumns; i++)
            {
                var columns = new Column(nbFloors, nbElevatorsByColumns, "Column " + i);
                //Console.WriteLine();
                this.columnList.Add(columns);
            }

        }

        // Screen display panel where people choose for his level to go
        // and show the number in screen of people choice
        
        public void DisplayMainPanel(int requestedFloorNumber)
        {        
            Console.WriteLine(requestedFloorNumber);            
        }

        // each columns have a range to deserved
        // and it's associate to people request

        public Column FindColumn(int requestedFloor)

        {
            if (requestedFloor >= 2 && requestedFloor <= 22)
            {
                return columnList[0];
            }
            else if (requestedFloor >= 23 && requestedFloor <= 43)
            {
                return columnList[1];
            }
            else if (requestedFloor >= 44 && requestedFloor <= 64)
            {
                return columnList[2];
            }
            else
            {
                return columnList[3];
            }
        }
    }
    //============================== Column =============================
    class Column
    {
        public int nbElevators;
        public int nbFloors;
        public string columnName;
        public int FloorNumber;
        public List<Elevator> elevatorList;
        public List<CallButton> callButtonList;
        public Column(int nbFloors, int nbElevators, string columnName)
        {
            this.nbElevators = nbElevators;
            this.nbFloors = nbFloors;
            this.columnName = columnName;
            callButtonList = new List<CallButton>();
            elevatorList = new List<Elevator>();
            this.CreateElevators(FloorNumber);
            this.CreateCallButtons();
        }
        // creating elevator and push it in elevator list
        public void CreateElevators(int FloorNumber)
        {
            for (int i = 0; i < this.nbElevators; i++)
            {
                var elevators = new Elevator("Elevator" + i, FloorNumber);
                this.elevatorList.Add(elevators);
            }
        }
        // the call button list is create with the number of floors
        // after they make a list and push it inside 

        public void CreateCallButtons()
        {
            for (var i = 0; i < this.nbFloors; i++)
            {
                var callbutton = new CallButton(i, "Down");
                if (i != 0)
                {
                    this.callButtonList.Add(callbutton);
                }
            }
        }
        // find the best elevator with requested floor number and number of floors
        // making the difference for send the best elevator for people request
        public Elevator FindNearestElevator(int requestedFloorNumber)
        {
            var newDiff = 27;
            Elevator nearestElevator = null;
            var oldDiff = 100;

            for (var i = 0; i < elevatorList.Count; i++)
            {
                newDiff = Math.Abs(
                    requestedFloorNumber - elevatorList[i].FloorNumber
                );
               
                if (newDiff < oldDiff){
                    nearestElevator = elevatorList[i];
                    oldDiff = newDiff;
                }
            }
        //    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine(nearestElevator.elevatorName);
            return nearestElevator;
        }
    }
    //=============================== CallButton======================================
    // button using the requested floor and direction of people requested
    class CallButton
    {
        public int requestedFloor;
        public string direction;
        public bool activateButton;
        public CallButton(int requestedFloor, string direction)

        {
            this.requestedFloor = requestedFloor;
            this.direction = "Down";
            this.activateButton = false;
        }
    }
    //========================================= Elevator =============================
    class Elevator
    {
        //class elevator using number of floor and direction
        // using open and closing doors
        // control the display panel where people choice is floor to go

        public string elevatorName;
        public int nbFloors;
        public string direction;
        public string status;
        public int FloorNumber;
        public List<int> requestFloorList;
        public Elevator(string elevatorName, int FloorNumber)
        {
            this.elevatorName = elevatorName;
            this.direction = "Stop";
            this.status = "Idle";
            this.FloorNumber = FloorNumber;
            requestFloorList = new List<int>();
        }
        public void MoveNext()
        {
            int requestedFloorNumber = requestFloorList[0];

                if (FloorNumber == requestedFloorNumber)
                {
                    // make move elevator with interval of 0.3 sec by floor level
                    // with requested floor number and Floor Number
                //    Console.WriteLine("TEST1");
                    Console.WriteLine("CallButtonLightOff");

                    OpeningAndClosingDoors();
                    requestedFloorNumber = ResetRequestedFloorNumber();
                }
                else if (requestedFloorNumber > FloorNumber)
                {
                //    Console.WriteLine("TEST2");
                    MoveUp(requestedFloorNumber);
                    System.Threading.Thread.Sleep(300);
                    Console.WriteLine(FloorNumber);
                }
                else
                {
                //    Console.WriteLine("TEST3");
                    MoveDown(requestedFloorNumber);
                    System.Threading.Thread.Sleep(300);
                    Console.WriteLine(FloorNumber);
                }
            // }
            Console.WriteLine("CallButtonLighton");
            ResetElevatorToFirstLobby();
            Console.WriteLine("CallButtonLightOff");
            OpeningAndClosingDoors();
        }
        //making a timer in 0.4 sec for open door
        // and wait 1 sec for close door 
        public void OpeningAndClosingDoors()
        {
            Console.WriteLine("Ding!  Open door");
            System.Threading.Thread.Sleep(400);
            Console.WriteLine("Dong!  Close Door");
            System.Threading.Thread.Sleep(400);
        }
        //When the elevator is done is job with the task in the list
        //its gonna remove the floor from the request floor list
        private int ResetRequestedFloorNumber()
        {
            requestFloorList.RemoveAt(0);
            int requestedFloorNumber = 1;
            if (requestFloorList.Count != (0))
            {
                requestedFloorNumber = requestFloorList[0];
            }
            return requestedFloorNumber;

        }
        public void ResetElevatorToFirstLobby()
        {
            while (this.FloorNumber > 1)
            {
                MoveDown(1);
                System.Threading.Thread.Sleep(100);
                Console.WriteLine(FloorNumber);
            }
        }

        // move up 
        public void MoveUp(int requestedFloorNumber)
        {
            while(FloorNumber < requestedFloorNumber) {
                FloorNumber++;
                Console.WriteLine("MovingUp");
                Console.WriteLine(FloorNumber);
            }
            
        }
        // moving down 
        public void MoveDown(int requestedFloorNumber)
        {
            while(FloorNumber > requestedFloorNumber) {
                FloorNumber--;
                Console.WriteLine("MovingDown");
                Console.WriteLine(FloorNumber);
            }
        }
        // Add the requested of people and add it in request floor list
        // the requested floor is making by descending  

        public void AddFloorToList(int requestedFloorNumber)
        {
            requestFloorList.Add(requestedFloorNumber);
            requestFloorList = requestFloorList.OrderByDescending(x => x).ToList();
        }
    }
}
