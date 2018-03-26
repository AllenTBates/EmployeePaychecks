using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeePaychecks
{
    /*
     * Date: 26 March 2018
     * Author: Allen Bates
     * Description: Program.cs is the main UI for the EmployeePaychecks app.
     * Here, the program will begin by reading Employees.txt, storing each
     * employee and then creating Paychecks.txt, TopEarners.txt and States.txt.
     * These files are saved in the bin/Debug folder of the project.
     * After these files are created, the user can then search for 
     * employees data by entering an employee ID.
     */

    class Program
    {
        //Method for creating Paychecks.txt
        public static void CreatePaychecksFile(List<Employee> employees, String filePath)
        {
            String line;

            //Sort list by gross pay
            List<Employee> employeesSorted = employees.OrderByDescending(o => o.grossPay).ToList();
            try
            {
                //Create Paychecks.txt file
                Console.Out.WriteLine("Creating Paychecks.txt file");
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine("Employee Paychecks");

                //Write employee's data to Paychecks.txt file
                foreach (Employee e in employeesSorted)
                {
                    line = "Employee Id: " + e.id +
                        " First Name: " + e.firstName +
                        " Last Name: " + e.lastName +
                        " Gross Pay: $" + e.grossPay +
                        " Federal Tax: $" + e.federalTax +
                        " State Tax: $" + e.stateTax +
                        " Net Pay: $" + e.netPay;
                    sw.WriteLine(line);
                }
                sw.Close();
                Console.Out.WriteLine("Paycheck.txt has been created");
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            
        }
        
        //Method for creating TopEarners.txt
        public static void CreateTopEarnersFile(List<Employee> employees, String filePath)
        {
            String line;

            //Get 15% of employees
            int topEarnersCount = Convert.ToInt32(
                Convert.ToDouble(employees.Count) * 0.15);
            //Create list of top earners
            List<Employee> topEarners = employees.OrderByDescending(o => o.grossPay).
                ToList().GetRange(0, topEarnersCount);
            
            List<Employee> employeesSorted;
            
            try
            {
                //Create TopEarners.txt file
                Console.Out.WriteLine("Creating TopEarners.txt file");
                StreamWriter sw = new StreamWriter(filePath);

                //Write list of employees by years worked
                sw.WriteLine("Top Earners");
                //Order employees by years, then last name, then first name
                employeesSorted = topEarners.OrderByDescending(o => o.yearsWorked)
                    .ThenBy(o=>o.lastName)
                    .ThenBy(o=>o.firstName).ToList();

                foreach (Employee e in employeesSorted)
                {
                    line = "First Name: " + e.firstName +
                        " Last Name: " + e.lastName +
                        " Years Worked: " + e.yearsWorked +
                        " Gross Pay: $" + e.grossPay;
                    sw.WriteLine(line);
                }

                sw.Close();
                Console.Out.WriteLine("TopEarners.txt has been created");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        //Method for creating States.txt
        public static void CreateStatesFile(List<Employee> employees, String filePath)
        {
            String line;

            //Create list of states
            List<State> states = new List<State>();
            foreach(Employee e in employees)
            {
                //Check if state is already in list
                if((states.Find(s => s.name.Equals(e.stateOfResidence))) != null)
                {
                    states.Find(s => s.name.Equals(e.stateOfResidence)).stateTimeWorked.Add(e.hoursWorkedInTwoWeeks);
                    states.Find(s => s.name.Equals(e.stateOfResidence)).stateNetPay.Add(e.netPay);
                    states.Find(s => s.name.Equals(e.stateOfResidence)).totalStateTaxesPaid += e.stateTax;
                }
                //If not add to list
                else
                {
                    State state = new State(e.stateOfResidence);
                    state.stateTimeWorked.Add(e.hoursWorkedInTwoWeeks);
                    state.stateNetPay.Add(e.netPay);
                    state.totalStateTaxesPaid += e.stateTax;
                    states.Add(state);
                }
            }
            
            //Sort the times worked and net pays for each state
            foreach(State s in states)
            {
                s.stateTimeWorked.Sort();
                s.stateNetPay.Sort();
            }

            try
            {
                //Create States.txt file
                Console.Out.WriteLine("Creating States.txt file");
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine("States");

                //Sort states alphabetically
                states = states.OrderBy(s => s.name).ToList();

                //Write state's data to States.txt file
                foreach (State s in states)
                {
                    line = "State: " + s.name +
                           " Median Time Worked: " + s.stateTimeWorked.ElementAt(s.stateTimeWorked.Count/2) +
                           " Median Net Pay: $" + s.stateNetPay.ElementAt(s.stateNetPay.Count/2) +
                           " Total State Taxes Paid: $" + s.totalStateTaxesPaid;
                    sw.WriteLine(line);
                }
                sw.Close();
                Console.Out.WriteLine("States.txt has been created");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            
        }

        static void Main(string[] args)
        {
            Console.Out.WriteLine("Searching for Employees.txt file");

            //Try opening Employees.txt file located in project directory
            try
            {
                List<Employee> employees = new List<Employee>();
                String line;

                //Start reading from file
                StreamReader sr = new StreamReader("Employees.txt");
                Console.Out.WriteLine("Reading from Employees.txt");
                line = sr.ReadLine();

                //Create first employee and add to list
                Employee firstEmployee = new Employee(line);
                employees.Add(firstEmployee);

                //Continue creating employees until EOF
                while(line != null)
                {
                    Employee employee = new Employee(line);
                    employees.Add(employee);
                    line = sr.ReadLine();
                }
                
                //Close file
                sr.Close();
                
                //Create Paychecks.txt
                CreatePaychecksFile(employees, "Paychecks.txt");

                //Create TopEarners.txt
                CreateTopEarnersFile(employees, "TopEarners.txt");

                //Creat States.txt
                CreateStatesFile(employees, "States.txt");

                //User interface for retrieving employee data based on id
                Console.Out.WriteLine("Would you like to lookup an employee? Yes/No");
                String reply = Console.In.ReadLine();
                while (!reply.Trim().Equals("no", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (reply.Trim().Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.Out.WriteLine("Please enter employee ID");
                        String id = Console.In.ReadLine();
                        if ((employees.Find(e => e.id.Equals(id))) != null)
                        {
                            Console.Out.WriteLine(employees.Find(e => e.id.Equals(id)).toString());
                        }
                        else
                        {
                            Console.Out.WriteLine("I'm sorry but that employee is not in our records");
                        }
                        Console.Out.WriteLine("Would you like to lookup another employee?");
                        reply = Console.In.ReadLine();
                    }
                    else
                    {
                        Console.Out.WriteLine("Sorry. You must reply with either 'Yes' or 'No'");
                        reply = Console.In.ReadLine();
                    }
                    
                }
                
                Console.Out.WriteLine("Goodbye");
            }
            catch(System.IO.FileNotFoundException e)
            {
                Console.Out.WriteLine("Exception: " + e.Message);
            }
            
            Console.ReadLine();
        }


    }
}
