using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePaychecks
{
    /*
     * Date: 26 March 2018
     * Author: Allen Bates 
     * Description: The Employee class defines the Employee object 
     * created when reading in each line of the Employees.txt file. 
     */
    class Employee 
    {
        public String id;
        public String firstName;
        public String lastName;
        public String payType;
        public Decimal salary;
        public DateTime startDate;
        public String stateOfResidence;
        public int hoursWorkedInTwoWeeks;
        public Decimal federalTax;
        public Decimal stateTax;
        public Decimal grossPay;
        public Decimal netPay;
        public int yearsWorked;

        //Empty Constructor
        public Employee()
        {

        }

        //Main Constructor
        public Employee(String line)
        {
            String[] employeeData = line.Split(',');
            this.id = employeeData[0].Trim();
            this.firstName = employeeData[1].Trim();
            this.lastName = employeeData[2].Trim();
            this.payType = employeeData[3].Trim();
            Decimal.TryParse(employeeData[4].Trim(), out this.salary);
            DateTime.TryParse(employeeData[5].Trim(), out this.startDate);
            this.stateOfResidence = employeeData[6].Trim();
            Int32.TryParse(employeeData[7].Trim(), out this.hoursWorkedInTwoWeeks);
            this.yearsWorked = DateTime.Now.Year - startDate.Year;
            /*
            Console.Out.WriteLine("Id: " + this.id + " First Name: " + this.firstName
                                    + " Last Name: " + this.lastName + " Pay Type: " + this.payType
                                    + " Salary: "+ this.salary + " Start Date: " +this.startDate.Month
                                    + "/" + this.startDate.Day + "/" + this.startDate.Year+" State: " + this.stateOfResidence 
                                    + " Hours Worked in Two Weeks: " + this.hoursWorkedInTwoWeeks);
            */
            CalculatePaycheck();
        }

        //Calculates taxes and pays for employee
        public void CalculatePaycheck()
        {
            decimal pay = this.salary;
            decimal fiftyPercentIncrease = 1.50m;
            decimal seventyFivePercentIncrease = 1.75m;
            decimal federalTaxRate = 0.15m;
            decimal fivePercentTaxRate = 0.05m;
            decimal sixPointFivePercentTaxRate = 0.065m;
            decimal sevenPercentTaxRate = 0.07m;

            //Calculate Gross Pay for Hourly Employees
            if (this.payType.Equals("H"))
            {
                if(this.hoursWorkedInTwoWeeks <= 80)
                {
                    this.grossPay = pay * this.hoursWorkedInTwoWeeks;
                }
                else if(this.hoursWorkedInTwoWeeks <= 90)
                {
                    this.grossPay = (pay * 80) +
                        (pay * fiftyPercentIncrease * (this.hoursWorkedInTwoWeeks-80));
                }
                else
                {
                    this.grossPay = (pay * 80) +
                        (pay * fiftyPercentIncrease * 10) +
                        (pay * seventyFivePercentIncrease * (this.hoursWorkedInTwoWeeks - 90));
                }
            }
            //Calculate Gross Pay for Salaried Employees
            //If paycheck is for salaried employees,
            //split their annual income for two week paychecks
            else if (this.payType.Equals("S"))
            {
                this.grossPay = pay / 26m;
            }
            else
            {
                Console.Out.WriteLine("Error in Employee Paytype. Neither Hourly or Salaried");
            }
            //Round Gross Pay
            this.grossPay = Math.Round(this.grossPay, 2);

            //Calculate Net Pay
            //Apply Federal Tax Rate and round it
            this.federalTax = this.grossPay * federalTaxRate;
            this.federalTax = Math.Round(this.federalTax, 2);

            //Apply State Tax Rate
            if(this.stateOfResidence.Equals("UT") || this.stateOfResidence.Equals("WY")
                || this.stateOfResidence.Equals("NV"))
            {
                this.stateTax = this.grossPay * fivePercentTaxRate;
            }
            else if (this.stateOfResidence.Equals("CO") || this.stateOfResidence.Equals("ID")
                || this.stateOfResidence.Equals("AZ") || this.stateOfResidence.Equals("OR"))
            {
                this.stateTax = this.grossPay * sixPointFivePercentTaxRate;
            }
            else if (this.stateOfResidence.Equals("WA") || this.stateOfResidence.Equals("NM")
                || this.stateOfResidence.Equals("TX"))
            {
                this.stateTax = this.grossPay * sevenPercentTaxRate;
            }

            //Round state tax
            this.stateTax = Math.Round(this.stateTax, 2);
            //Apply taxes
            this.netPay = this.grossPay - this.federalTax - this.stateTax;
            //Round NetPay
            this.netPay = Math.Round(this.netPay, 2);

            
        }

        //Returns Employee Data
        public String toString()
        {
            return "First Name: " + this.firstName +
                   " Last Name: " + this.lastName +
                   " Pay Type: " + this.payType +
                   " Salary: " + this.salary +
                   " Start Date: " + this.startDate.ToString() +
                   " State of Residence: " + this.stateOfResidence +
                   " Hours Worked in Two Weeks: " + this.hoursWorkedInTwoWeeks +
                   " Federal Tax: $" + this.federalTax +
                   " State Tax: $" + this.stateTax +
                   " Gross Pay: $" + this.grossPay +
                   " Net Pay: $" + this.netPay +
                   " Years Worked: " + this.yearsWorked;
        }
    }
}
