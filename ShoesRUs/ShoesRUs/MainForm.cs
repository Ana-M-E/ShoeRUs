using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ShoesRUs
{
    public partial class MainForm : Form
    {
        Login login = new Login();
        Register register = new Register();
        public MainForm()
        {
            InitializeComponent();
            Startup su = new Startup();
            grpProfile.Visible = false;
            grpLogin.Visible = false;
            grpRegister.Visible = false;
            grpBasket.Visible = false;
            grpViewProduct.Visible = false;
            grpMain.Visible = false;
            grpContact.Visible = false;
            
        }



        /* /////////////////////////////CONTACT GROUP BOX/////////////////////////////////////////////////////*/

        


        OleDbConnection myConn = new OleDbConnection();

        private void btnShowContact_Click(object sender, EventArgs e)//shows CONTACT US page (group box), setting the visibility of any other group boxes to false
        {
            grpContact.Visible = true;
            grpProfile.Visible = false;
            grpLogin.Visible = false;
            grpRegister.Visible = false;
            grpBasket.Visible = false;
            grpViewProduct.Visible = false;
            grpMain.Visible = false;
            
        }

        private void btnSend_Click(object sender, EventArgs e)//sends a message to the database
        {

            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
               string.IsNullOrEmpty(txtSubj.Text) || string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("One or more fields are empty.");
            }
            else
            {

                Contact contact = new Contact();
                contact.sendMessage(txtName.Text, txtEmail.Text, txtCustNo.Text, txtOrdNo.Text, cmbCategory.SelectedItem.ToString(), txtSubj.Text, txtMessage.Text);
                int chkMessage = contact.checkMessage(txtName.Text, txtEmail.Text, txtCustNo.Text, txtOrdNo.Text, cmbCategory.SelectedItem.ToString(), txtSubj.Text, txtMessage.Text);
                if (chkMessage == 1)
                {
                    MessageBox.Show("Message successfully sent!");
                }
                else
                {
                    
                    MessageBox.Show("Error when sending message!");

                }

                clearFieldsContactForm();


            }
        }

        private void btnReset_Click(object sender, EventArgs e)//clear all the fields to be completed (Contact group box)
        {
            clearFieldsContactForm();
        }

        private void clearFieldsContactForm()//function which clear all the fields to be completed (Contact group box)
        {

            txtName.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtCustNo.Clear();
            txtOrdNo.Clear();
            txtSubj.Clear();
            txtMessage.Clear();
            cmbCategory.SelectedIndex = -1;

        }





        /* /////////////////////////////PROFILE GROUP BOX/////////////////////////////////////////////////////*/

        private void btnShowProfile_Click(object sender, EventArgs e)//shows MY PROFILE page (group box), setts the visibility of all the other group boxes to false
        {
            grpProfile.Visible = true;
            grpLogin.Visible = false;
            grpRegister.Visible = false;
            grpBasket.Visible = false;
            grpViewProduct.Visible = false;
            grpMain.Visible = false;
            grpContact.Visible = false;
        }


        /* ---------------------------GENERAL INFO UPDATE GROUP BOX-------------------------------------------------*/

        private void btnViewProfileDetails_Click(object sender, EventArgs e)//On "My profile" group box, this button displays the group box which includes the general profile details
        {
            grpProfileDetails.Visible = true;
            grpAddressUpdate.Visible = false;
            grpCardUpdate.Visible = false;
            grpPurchases.Visible = false;
        }

        private void btnOKGeneralInfo_Click(object sender, EventArgs e)//displays the general information about the Customer (which is hold in a group box called grpGeneralInfoProfile
        {
            grpGeneralInfoProfile.Visible = true;
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "Select CustomerTitle, CustomerDOB, CustomerGender, CustomerName, CustomerPhoneNo, CustomerEmail From Customer"
                                                           + " Where CustomerID = @customerID";
                myCmd.Parameters.AddWithValue("customerID", txtAddIDProfile.Text);

                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();
                myDR.Read();

                //extract information and display through the UI
                txtTitleProfile.Text = myDR[0].ToString();
                txtDOBProfile.Text = myDR[1].ToString();
                txtGenderProfile.Text = myDR[2].ToString();
                txtNameProfile.Text = myDR[3].ToString();
                txtPhoneProfile.Text = myDR[4].ToString();
                txtEmailProfile.Text = myDR[5].ToString();

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

        }

        private void btnUpdateGeneralInfoProfile_Click(object sender, EventArgs e)//button which updates the general information about the customer on the database
        {
            try
            {
                

                myConn.ConnectionString = DatabaseConnection.dbconnect;
                myConn.Open();

                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "UPDATE Customer SET CustomerTitle = @ct, CustomerGender = @cGender, CustomerName = @cName, CustomerPhoneNo = @cPhone"
                                                           + " Where CustomerID = " + txtAddIDProfile.Text;
                myCmd.Parameters.AddWithValue("@ct", txtTitleProfile.Text);
                myCmd.Parameters.AddWithValue("@cGender", txtGenderProfile.Text);
                myCmd.Parameters.AddWithValue("@cName", txtNameProfile.Text);
                myCmd.Parameters.AddWithValue("@cPhone", txtPhoneProfile.Text);
                
                

                int rowsChanged = myCmd.ExecuteNonQuery();

                myConn.Close();

                clearFieldsGenetalInfo();

                MessageBox.Show("Successfully updated! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearGeneralInfo_Click(object sender, EventArgs e)//clears all the fields in the general info about customer group box
        {
            clearFieldsGenetalInfo();
        }

        private void clearFieldsGenetalInfo()//function which clears all the fields in the GENERAL INFORMATION group box (My profile)
        {

            txtTitleProfile.Clear();
            txtGenderProfile.Clear();
            txtNameProfile.Clear();
            txtPhoneProfile.Clear();
            txtAddIDProfile.Clear();
            txtEmailProfile.Clear();
            txtDOBProfile.Clear();
           

        }



        /* ---------------------------ADDRESS UPDATE GROUP BOX---------------------------------------------------------------------*/


        private void btnShowUpdateAddress_Click(object sender, EventArgs e)//displays the update address groupbox, inside my profile
        {
            grpProfileDetails.Visible = false;
            grpAddressUpdate.Visible = true;
            grpCardUpdate.Visible = false;
            grpPurchases.Visible = true;
        }

        private void btnOKAddress_Click(object sender, EventArgs e)//displays the fields to be completed by the user in other to update the address details
        {
            grpAddressUpdateInfo.Visible = true;
            
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "Select CustomerAddressNo, CustomerAddressStreet, CustomerAddressCity, CustomerAddressCountry, CustomerPostCode, CustomerPhoneNo From Customer"
                                                           + " Where CustomerID = @customerID";
                myCmd.Parameters.AddWithValue("customerID", txtAddIDAddress.Text);

                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();
                myDR.Read();

                //extract information and display through the UI
                txtHouseNoProfile.Text = myDR[0].ToString();
                txtStreetProfile.Text = myDR[1].ToString();
                txtCityProfile.Text = myDR[2].ToString();
                txtCountryProfile.Text = myDR[3].ToString();
                txtPostcodeProfile.Text = myDR[4].ToString();
                

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        private void btnUpdateAddress_Click(object sender, EventArgs e)//update the new address information to the database
        {
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect;
                myConn.Open();

                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "UPDATE Customer SET CustomerAddressNo = @ca, CustomerAddressStreet = @caStreet, CustomerAddressCity = @caCity, CustomerAddressCountry = @caCountry, CustomerPostCode = @caPostcode "
                                                           + " Where CustomerID = " + txtAddIDAddress.Text;
                myCmd.Parameters.AddWithValue("@caNo", txtHouseNoProfile.Text);
                myCmd.Parameters.AddWithValue("@caStreet", txtStreetProfile.Text);
                myCmd.Parameters.AddWithValue("@caCity", txtCityProfile.Text);
                myCmd.Parameters.AddWithValue("@caCountry", txtCountryProfile.Text);
                myCmd.Parameters.AddWithValue("@caPostcode", txtPostcodeProfile.Text);
                

                int rowsChanged = myCmd.ExecuteNonQuery();

                myConn.Close();

                MessageBox.Show("Successfully updated! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearAddress_Click(object sender, EventArgs e)//clear fields in the address group box
        {
            clearFieldsAddress();
        }

        private void clearFieldsAddress()
        {

            txtAddIDAddress.Clear();
            txtHouseNoProfile.Clear();
            txtStreetProfile.Clear();
            txtCityProfile.Clear();
            txtCountryProfile.Clear();
            txtPostcodeProfile.Clear();
          
        }




        /* ---------------------------CARD UPDATE GROUP BOX----------------------------------------------------------*/



        private void btnShowUpdateCardDetails_Click(object sender, EventArgs e)//shows the update card group box inside My profile
        {
           
            grpProfileDetails.Visible = false;
            grpAddressUpdate.Visible = false;
            grpCardUpdate.Visible = true;
            grpPurchases.Visible = false;
        }

        private void btnOKUpdateCardInfo_Click(object sender, EventArgs e)//shows the group box which contains the card details information
        {
            grpUpdateCardInfo.Visible = true;
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "Select  CustomerPaymentCardType, CustomerPaymentCardNo, CustomerPaymentCardCVV, CustomerPaymentCardName,CustomerPaymentCardExpDate From Customer"
                                                           + " Where CustomerID = @customerID";
                myCmd.Parameters.AddWithValue("customerID", txtCustomerIDCardProfile.Text);

                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();
                myDR.Read();

                //extract information and display through the UI
                txtCardTypeProfile.Text = myDR[0].ToString();
                txtCardNoProfile.Text = myDR[1].ToString();
                txtCVVProfile.Text = myDR[2].ToString();
                txtHolderProfile.Text = myDR[3].ToString();
                txtExpDateProfile.Text = myDR[4].ToString();

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }


        private void btnUpdateCardDetails_Click(object sender, EventArgs e)//updates the card information introduced in the fields by the user
        {
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect;
                myConn.Open();

                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "UPDATE Customer SET CustomerPaymentCardType = @cpType , CustomerPaymentCardNo = @cpCardNo, CustomerPaymentCardCVV = @cpCVV, CustomerPaymentCardName = @cpHolder,CustomerPaymentCardExpDate = @cpExpDate"
                                                           + " Where CustomerID = " + txtCustomerIDCardProfile.Text;
                myCmd.Parameters.AddWithValue("@cpType", txtCardTypeProfile.Text);
                myCmd.Parameters.AddWithValue("@cpCardNo", txtCardNoProfile.Text);
                myCmd.Parameters.AddWithValue("@cpCVV",  txtCVVProfile.Text);
                myCmd.Parameters.AddWithValue("@cpHolder", txtHolderProfile.Text);
                myCmd.Parameters.AddWithValue("@cpExpDate", txtExpDateProfile.Text);


                int rowsChanged = myCmd.ExecuteNonQuery();

                myConn.Close();

                clearFieldsCardNo();

                MessageBox.Show("Successfully updated! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearCard_Click(object sender, EventArgs e)//clears the fields in the update card detail group box
        {
            clearFieldsCardNo();
        }

        private void clearFieldsCardNo()//function which clears all the fields in the GENERAL INFORMATION group box (My profile)
        {

            txtCustomerIDCardProfile.Clear();
            txtCardTypeProfile.Clear();
            txtCardNoProfile.Clear();
            txtExpDateProfile.Clear();
            txtCVVProfile.Clear();
            txtHolderProfile.Clear();

        }




        /* --------------------------------------------PURCHASES GROUP BOX----------------------------------------------------------*/


        private void btnViewPurchases_Click(object sender, EventArgs e)//shows the group box for purchases 
        {
            grpPurchases.Visible = true;
            grpProfileDetails.Visible = false;
            grpAddressUpdate.Visible = false;
            grpCardUpdate.Visible = false;

        }

        private void btnOKPurchasesDisplay_Click(object sender, EventArgs e)//displays the list of orders
        {
            grpListPurchasesProfile.Visible = true;
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "SELECT Orders.OrderID, Orders.OrderDate FROM  Orders, Invoice WHERE  Invoice.OrderID=Orders.OrderID AND Invoice.CustomerID = " + txtIDCustomerPurchases.Text;

                MessageBox.Show(myCmd.CommandText);


                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();


                lstView.View = View.Details;

                while (myDR.Read())
                {
                    var item = new ListViewItem();
                    item.Text = myDR["OrderID"].ToString();
                    item.SubItems.Add(myDR["OrderDate"].ToString());


                    lstView.Items.Add(item);
                }


                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }






























        private void btnAdmin_Click(object sender, EventArgs e)
        {

        }

       

        private void btnBasket_Click(object sender, EventArgs e)
        {
            grpBasket.Visible = true;
        }

        


        /* /////////////////////////////LOG IN GROUP BOX +++++ LOG OUT BUTTON/////////////////////////////////////////////////////*/
        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (login.loggingIn(txtLoginEmail.Text, txtLoginPassword.Text) != -999)
            {
                login.setLoggedIn(login.loggingIn(txtLoginEmail.Text, txtLoginPassword.Text));
                grpLogin.Visible = false;
                btnShowRegisterGrp.Visible = false;
                btnShowProfile.Visible = true;
                btnBasket.Visible = true;
                btnLogout.Visible = true;
                if (login.checkAdmin() == true)
                {
                    btnAdmin.Visible = true;
                }
                MessageBox.Show("Login successfull!");
                txtLoginEmail.Text = "";
                txtLoginPassword.Text = "";
            }
            else
            {
                MessageBox.Show("Login details incorrect!");
            }
        }


        //Shows the LoginForm
        private void btnShowLoginGrp_Click(object sender, EventArgs e)
        {
            grpLogin.Visible = true;
        }

        /* /////////////////////////////REGISTRATION GROUP BOX/////////////////////////////////////////////////////*/

        private void btnShowRegisterGrp_Click(object sender, EventArgs e)
        {
            grpRegister.Visible = true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //register.register();

            //Check if any of the input boxes are empty or not selected
            if (cmbRegTitle.SelectedItem == null || cmbRegGender.SelectedItem == null || string.IsNullOrEmpty(txtRegName.Text) ||
                cmbRegCaType.SelectedItem == null || string.IsNullOrEmpty(txtRegEmail.Text) || string.IsNullOrEmpty(txtRegPassword.Text) ||
                string.IsNullOrEmpty(txtRegPasswordConfirm.Text) || string.IsNullOrEmpty(txtRegDOB.Text) || string.IsNullOrEmpty(txtRegPhoneNo.Text) ||
                string.IsNullOrEmpty(txtRegAddNo.Text) || string.IsNullOrEmpty(txtRegAddStreet.Text) || string.IsNullOrEmpty(txtRegAddCity.Text) ||
                string.IsNullOrEmpty(txtRedAddCountry.Text) || string.IsNullOrEmpty(txtRegPostCode.Text) || string.IsNullOrEmpty(txtRegCaName.Text) ||
                string.IsNullOrEmpty(txtRegCaNo.Text) || string.IsNullOrEmpty(txtRegCaCVV.Text) || string.IsNullOrEmpty(txtRegCaExpiry.Text))
            {
                MessageBox.Show("One or more fields are empty.");
            }
            else
            {
                //Check if the email entered already exists
                if (register.checkEmailExists(txtRegEmail.Text) == true)
                {
                    MessageBox.Show("This email address is already being used by another account.");
                }
                else
                {
                    //Check if the Date of Birth field is entered correctly
                    DateTime resultDOB;
                    if (DateTime.TryParseExact(txtRegDOB.Text, new string[] { "d-M-yyyy", "d/M/yyyy", "d.M.yyyy" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resultDOB))
                    {
                        //Check if the Card Expiry field is entered correctly
                        DateTime resultExpiry;
                        if (DateTime.TryParseExact(txtRegCaExpiry.Text, new string[] { "MM-yy", "MM/yy", "MM.yy" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resultExpiry))
                        {
                            //Check if Phone Number, Card Number and Card CVV input are numbers
                            if (Regex.IsMatch(txtRegCaNo.Text, @"^\d+$") || Regex.IsMatch(txtRegCaCVV.Text, @"^\d+$"))
                            {
                                if (txtRegPassword.Text != txtRegPasswordConfirm.Text)
                                {
                                    MessageBox.Show("Password doesn't match.");
                                }
                                else
                                {
                                    //Encryption for passwords
                                    Encryption ec = new Encryption();
                                    //Insert registration details into the database
                                    OleDbConnection dbCon = new OleDbConnection(DatabaseConnection.dbconnect);

                                    dbCon.ConnectionString = DatabaseConnection.dbconnect;
                                    OleDbCommand dbCmd = dbCon.CreateCommand();

                                    dbCmd.CommandText = "INSERT INTO Customer(CustomerTitle, CustomerName, CustomerDOB, CustomerGender, CustomerEmail, CustomerPhoneNo, CustomerAddressNo, CustomerAddressStreet, CustomerAddressCity, CustomerAddressCountry, CustomerPostCode, CustomerPaymentCardType, CustomerPaymentCardNo, CustomerPaymentCardCVV, CustomerPaymentCardName, CustomerPaymentCardExpDate, CustomerPassword) VALUES (@CustomerTitle, @CustomerName, @CustomerDOB, @CustomerGender, @CustomerEmail, @CustomerPhoneNo, @CustomerAddressNo, @CustomerAddressStreet, @CustomerAddressCity, @CustomerAddressCountry, @CustomerPostCode, @CustomerPaymentCardType, @CustomerPaymentCardNo, @CustomerPaymentCardCVV, @CustomerPaymentCardName, @CustomerPaymentCardExpDate, @CustomerPassword)";

                                    dbCmd.Parameters.AddWithValue("CustomerTitle", cmbRegTitle.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerName", txtRegName.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerDOB", resultDOB.ToShortDateString());
                                    dbCmd.Parameters.AddWithValue("CustomerGender", cmbRegGender.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerEmail", txtRegEmail.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPhoneNo", txtRegPhoneNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressNo", txtRegAddNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressStreet", txtRegAddStreet.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressCity", txtRegAddCity.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressCountry", txtRedAddCountry.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPostCode", txtRegPostCode.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardType", cmbRegCaType.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardNo", txtRegCaNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardCVV", txtRegCaCVV.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardName", txtRegCaName.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardExpDate", resultExpiry.ToShortDateString());
                                    dbCmd.Parameters.AddWithValue("CustomerPassword", ec.Encrypt(txtRegPassword.Text));

                                    dbCon.Open();
                                    int rowsChanged = dbCmd.ExecuteNonQuery();
                                    dbCon.Close();

                                    MessageBox.Show("Registration Successful!");
                                    register.clearFields();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Card Number or Card CVV is not a number.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Card Expiry field entered incorrect. Use the format MM/YY.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Date of Birth field entered incorrect. Use the format DD/MM/YYYY.");
                    }
                }
            }
        }

        private void btnCancelRegister_Click(object sender, EventArgs e)
        {
            register.clearFields();
        }

        
    }
}
