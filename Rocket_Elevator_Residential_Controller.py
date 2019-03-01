import time 
class battery(object):
    def __init__(self, ColumnNumber, nbElevator):
       self.status = "OPERATIONAL"
       self.columnnumber = ColumnNumber 
       self.column_list = []
       self.gen_column()
    def gen_column(self):
       for x in range(self.columnnumber):
           self.column_list.append(Column("Column"+str(x+1), "Operational", 10, 2))
class Callbutton(object):
    def __init__ (self, Floor, Direction, Status):
        self.floor = Floor
        self.Direction = Direction
        self.status = Status
        self.light = "off"
        # print(self.light)
    def turnCallbuttonOn(self):
        self.light = "on"
        self.status = "Active"
        print(self.status)
    def turnCallbuttonOff(self):
        self.light = "off"
class FloorButton(object):
    def __init__ (self, Floor, Status):
        self.floor = Floor
        self.status = Status 
class Column(object):
    def __init__(self,Name, Status, floornumber, nbElevator):
        self.name = Name
        self.floornumber = floornumber
        self.nbElevator = nbElevator
        self.status = "Operational"
        self.ElevatorList = []
        self.genElevator()
        self.CallButtonList = []
        self.genCallButtons ()
        print(self.floornumber)
    
    def genElevator(self):
        x = 0
        for x in range(self.nbElevator):
            self.ElevatorList.append(Elevator(x+1, "Idle", "None", 1, self.floornumber,"Close", [], 1))
    def genCallButtons(self): 
        for i in range(self.floornumber):
            if i!=(self.floornumber-1):
                self.CallButtonList.append(Callbutton(i+1, "GoingUp", "Idle"))
            if i!=0: 
                self.CallButtonList.append(Callbutton(i+1, "GoingDown", "Idle"))
    def callElevator(self, floor, Direction):
        print("call elevator")
        target_button = self.findCallButton(floor, Direction)
        target_button.turnCallbuttonOn()  #Turn the call button light on     
        elevator = self.findBestElevator(floor, Direction)   #found best elevator
        elevator.floorlist.append(floor)
        return elevator
        
    def findCallButton(self, floor, Direction):
        target_button = None
        for button in self.CallButtonList:
            if button.floor == floor and button.Direction == Direction: 
                target_button = button
                print("FOUND BUTTON")
        return target_button
    def OperateElevator(self):
        for elevator in self.ElevatorList:
            while len(elevator.floorlist) > 0:
                if elevator.floor == elevator.floorlist[0]:
                    elevator.status = "Stopped"
                    elevator.floorlist.pop(0)
                    elevator.DoorOpen()
                    time.sleep(3)
                    elevator.CloseDoor()
                elif elevator.floor > elevator.floorlist[0]:
                    elevator.status = "Moving"
                    elevator.Direction = "GoingDown"
                    elevator.MoveDown(elevator.floorlist[0])
                elif elevator.floor < elevator.floorlist[0]:
                    elevator.status = "Moving"
                    elevator.Direction = "GoingUp"
                    elevator.MoveUp(elevator.floorlist[0])
            elevator.status = "Idle"
            elevator.Direction = "GoingNoWhere"


    def findBestElevator(self, floor, Direction):
        target_elevator = None
        for elevator in self.ElevatorList:
            target_elevator = elevator
            if floor == elevator.floor:
                print("FOUND ELEVATOR")   
                if elevator.status == "Stopped" and elevator.Direction == Direction:
                    print(elevator.status)
                    return target_elevator   
                elif elevator.status == "Idle":
                    return target_elevator
                elif floor > elevator.floor:
                    if elevator.status is "GoingUp" and Direction == elevator.Direction:
                        return target_elevator
                elif elevator.status == "Stopped" and elevator.Direction is "GoingUp" and Direction == elevator.Direction:
                    return target_elevator
                elif floor < elevator.floor: 
                    if elevator.status is "GoingDown" and Direction == elevator.Direction:
                        return target_elevator
            elif elevator.status == "Stopped" and elevator.Direction is "GoingDown" and Direction == elevator.Direction:
                return target_elevator
            elif elevator.status == "Idle":
                #find nearest
                return target_elevator 

class Elevator(object):
    def __init__(self, Name, Status, Direction, Origin, NbFloors, Door, Floorlist, Floor):
        self.name = Name
        self.status = Status
        self.Direction = Direction
        self.origin = Origin
        self.NbFloors = NbFloors
        self.door = "Closed"
        self.floorlist = []
        self.floor = Floor
        self.floorbuttons = []
        self.genFloorButton()
        #Generate the buttons inside the elevator
    def genFloorButton(self):
        for x in range(self.NbFloors):
            self.floorbuttons.append(FloorButton(x+1, "Idle"))
        #makes the elevator move
    def MoveUp(self, floor):
        while self.floor < floor:
            self.floor += 1
            print("Elevator #" + str(self.name) + "now at floor" + str(self.floor))
    def MoveDown(self, floor):
        while self.floor > floor:
            self.floor -= 1
            print("Elevator #" + str(self.name) + " now at floor" + str(self.floor))
        #makes the elevator open or close their doors
    def DoorOpen(self):
        self.door = "open"
        print("Open Door")
    def CloseDoor(self):
        self.door = "close"
        print("Close Door")
    def RequestFloor(self, requestFloor):
        self.floorlist.append(requestFloor)
        
    #bouton interieur 
    #move elevator



print("depart")
column = Column(1, "Operational", 10,2)
elevator = column.callElevator(5, "GoingDown")
column.OperateElevator()
elevator.RequestFloor(3)
column.OperateElevator()

 