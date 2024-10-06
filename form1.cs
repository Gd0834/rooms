using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RoomBookingApp
{
    public partial class Form1 : Form
    {
        // Dictionary to store the room's seat count and their booking status
        private Dictionary<string, List<bool>> roomSeatStatus = new Dictionary<string, List<bool>>();

        // ComboBox for room selection
        private ComboBox roomSelectionComboBox;

        public Form1()
        {
            InitializeComponent(); // Calls the method in the Designer file to initialize controls

            // Initialize room selection dropdown
            InitializeRoomSelection();

            // Initialize rooms and seats
            InitializeRooms();
        }

        // Method to initialize the room selection ComboBox
        private void InitializeRoomSelection()
        {
            roomSelectionComboBox = new ComboBox();
            roomSelectionComboBox.Dock = DockStyle.Top;
            roomSelectionComboBox.SelectedIndexChanged += RoomSelectionComboBox_SelectedIndexChanged;

            // Add ComboBox to the form controls
            this.Controls.Add(roomSelectionComboBox);
        }

        // Event handler for room selection change
        private void RoomSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRoom = roomSelectionComboBox.SelectedItem.ToString();
            LoadRoomSeats(selectedRoom);
        }

        // Method to initialize rooms and their seat counts
        private void InitializeRooms()
        {
            // Example rooms with different number of seats
            roomSeatStatus["Room 1"] = new List<bool> { true, true, true, true, true }; // 5 seats
            roomSeatStatus["Room 2"] = new List<bool> { true, true, true };             // 3 seats
            roomSeatStatus["Room 3"] = new List<bool> { true, true, true, true };       // 4 seats

            // Add rooms to the ComboBox for selection
            roomSelectionComboBox.Items.AddRange(new object[] { "Room 1", "Room 2", "Room 3" });
            roomSelectionComboBox.SelectedIndex = 0; // Default to first room
        }

        // Method to load seats for a selected room
        private void LoadRoomSeats(string roomName)
        {
            // Clear existing seat buttons from the table layout
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.ColumnCount = 5; // Assume max 5 seats per row

            List<bool> seatStatusList = roomSeatStatus[roomName];
            int totalSeats = seatStatusList.Count;
            int seatCounter = 1;

            // Add rows based on the number of seats
            tableLayoutPanel1.RowCount = (int)Math.Ceiling(totalSeats / 5.0);

            // Loop to create buttons and add to the TableLayoutPanel
            for (int i = 0; i < totalSeats; i++)
            {
                Button seatButton = new Button();
                string seatId = $"Seat {seatCounter}";

                seatButton.Text = seatId;
                seatButton.BackColor = seatStatusList[i] ? Color.Green : Color.Red; // Available or booked seat color
                seatButton.Click += SeatButton_Click;

                // Track button's index to know which seat to toggle
                seatButton.Tag = new Tuple<string, int>(roomName, i);

                // Add button to TableLayoutPanel
                tableLayoutPanel1.Controls.Add(seatButton, i % 5, i / 5); // 5 seats per row
                seatCounter++;
            }
        }

        // Event handler for seat button clicks
        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button seatButton = sender as Button;
            var tag = seatButton.Tag as Tuple<string, int>;

            string roomName = tag.Item1;
            int seatIndex = tag.Item2;

            // Get seat status for the selected room
            List<bool> seatStatusList = roomSeatStatus[roomName];

            // Check if the seat is available
            if (seatStatusList[seatIndex])
            {
                // Confirm the booking
                DialogResult result = MessageBox.Show($"Do you want to book {seatButton.Text} in {roomName}?", 
                                                      "Confirm Booking", 
                                                      MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    seatStatusList[seatIndex] = false; // Mark the seat as booked
                    seatButton.BackColor = Color.Red; // Change button color to indicate booking
                    MessageBox.Show($"{seatButton.Text} in {roomName} has been booked!");
                }
            }
            else
            {
                MessageBox.Show($"{seatButton.Text} in {roomName} is already booked.");
            }
        }
    }
}
