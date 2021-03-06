﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePaychecks
{
    /*
     * Date: 26 March 2018
     * Author: Allen Bates
     * Description: The State class is used to define each
     * state that the employees of Employees.txt work in.
     * 
     */
    class State
    {
        public String name { get; set; }
        public List<int> stateTimeWorked { get; set; }
        public List<Decimal> stateNetPay { get; set; }
        public Decimal totalStateTaxesPaid { get; set; }

        //Empty constructor
        public State()
        {

        }

        //Default constructor
        public State(String name)
        {
            this.name = name;
            this.stateTimeWorked = new List<int>();
            this.stateNetPay = new List<decimal>();
            this.totalStateTaxesPaid = 0.0m;
        }
    }
}
