﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ppcLookupV2
{
    public partial class SendEdit : Window
    {
        public SendEdit()
        {
            InitializeComponent();
            requestCbox.Items.Add("Add New Listing");
            requestCbox.Items.Add("Add Town to Existing County");
            requestCbox.Items.Add("Change Code for Existing Listing");
        }

        private void sendRequest_Click(object sender, RoutedEventArgs e)
        {

            Request edit = new Request();
            int n;

            edit.Task = requestCbox.Text;
            edit.State = stateBox.Text;
            edit.County = countyBox.Text;
            edit.Town = townBox.Text;
            bool protectCode = int.TryParse(codeBox.Text, out n);
            if (protectCode)
            {
                edit.Code = Convert.ToInt32(codeBox.Text);
            }
            else
            {
                validateInput();
            }
            
            if (validateInput() == true)
            {
                edit.sendRequest();
                this.Close();
                MessageBox.Show("Request sent successfully!");
            }
            else
            {
                
                MessageBox.Show(@"Check your inputs! All fields must be filled!
                  - Task type
                  - State
                  - County
                  - Town
                  - Code (between 1 and 10)", "Error");
            }
        }

        // Error checking: this thing needs to be airtight and scream at the slightest
        // incorrect input aka if anything doesn't fit, throw up a message box
        private bool validateInput()
        {
            int n; // var for TryParse
            bool checkTask = requestCbox.SelectedIndex == -1; // Check if Task Combobox has a selection
            bool checkState = string.IsNullOrEmpty(stateBox.Text); // Check if State Textbox is null/empty
            bool checkCounty = string.IsNullOrEmpty(countyBox.Text); // Check if County Textbox is null/empty
            bool checkTown = string.IsNullOrEmpty(townBox.Text); // Check if Town Textbox is null/empty
            bool checkCodeEmpty = string.IsNullOrEmpty(codeBox.Text); // Check if Code Textbox is null/empty
            bool codeIsNumeric = int.TryParse(codeBox.Text, out n); // Check if code in box can be parsed to int
            bool codeInRange = false; // var for valid range of codes

            List<string> validCodes = new List<string>();
            for (int i = 1; i < 11; i++)
            {
                validCodes.Add($"{i}");
            }

            foreach (string code in validCodes)
            {
                if (code == codeBox.Text)
                {
                    codeInRange = true;
                }
            }

            if (checkTask == true)
                return false;
            else if (checkState == true)
                return false;
            else if (checkCounty == true)
                return false;
            else if (checkTown == true)
                return false;
            else if (checkCodeEmpty == true)
                return false;
            else if (codeIsNumeric == false)
                return false;
            else if (codeInRange == false)
                return false;
            else
                return true;
        }

        private void cancelRequest_Click(object sender, RoutedEventArgs e)
        {
            requestCbox.SelectedIndex = -1;
            stateBox.Clear();
            countyBox.Clear();
            townBox.Clear();
            codeBox.Clear();
            this.Close();
        }
    }
}
