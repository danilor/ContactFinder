using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagicContact;

namespace ContactFinder
{
    public partial class ContactInput : Form
    {
        public ContactInput()
        {
            InitializeComponent();
            this.ChangeEmailIcon(null, null);
            //this.txtEmail.Text = "youremail@domain.com";
        }

        /**
         * The search click button
         * */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            // We get the email from the textbox
            String email = this.txtEmail.Text;
            // Lets check if it is a valid email
            if (Common.Utilities.Validation.ValidEmail(email)) {
                //If it is valid, lets disable the form and search for the email
                this.enableForm();
                FullContact fullContact = new FullContact();
                bool success = fullContact.lookByEmail(email);
                if (success == false)
                {
                    this.ErrorEmailValidation();
                }
                else {
                    // If it arrives here, it means that the search was sucessfull and we can show something?
                    MagicContact.Contact.Contact contact = fullContact.getContact();
                    EngineSearch.Engines.Google GoogleEngine = new EngineSearch.Engines.Google();
                    
                    // We need to open the new window with the contact information
                    ContactResults ContactResult = new ContactResults(this, contact);
                    ContactResult.Show();
                    this.enableForm();
                    this.Hide();
                }
            }
            else
            {
                //if it is invalid, lets show the Alert ICON
                this.ErrorEmailValidation();
            }
            this.Cursor = Cursors.Default;
        }
        /**
            Lets disable all form elements
             */
        private void disableForm() {
            this.txtEmail.Enabled = false;
            this.btnSearch.Enabled = false;
        }
        /**
            Lets enable all form elements
             */

        private void enableForm() {
            this.txtEmail.Enabled = true;
            this.btnSearch.Enabled = true;
        }

        private void ErrorEmailValidation() {
            this.ErrorEmailValidationMessage( null );
        }

        private void ErrorEmailValidationMessage( String message ) {
            this.ChangeExclamationIcon();
            Timer MyTimer = new Timer();
            MyTimer.Interval = (4000);
            MyTimer.Tick += new EventHandler(ChangeEmailIcon);
            MyTimer.Start();
        }

        private void ChangeEmailIcon(object sender, EventArgs e)
        {
            this.faEmail.IconType = FontAwesomeIcons.IconType.Envelope;
            this.faEmail.ActiveColor = Color.Black;
            this.faEmail.InActiveColor = Color.Gray;
        }

        private void ChangeExclamationIcon() {
            Color c = Color.Red;
            this.faEmail.IconType = FontAwesomeIcons.IconType.ExclamationTriangle;
            this.faEmail.ActiveColor = c;
            this.faEmail.InActiveColor = c;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            this.ChangeEmailIcon( null , null );
        }

    }
}
