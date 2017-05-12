using System;
using System.Drawing;
using System.Windows.Forms;
using MagicContact.Contact;
using System.Diagnostics; // This is only for the Process.start to open the browser

namespace ContactFinder
{
    public partial class ContactResults : Form
    {
        private Contact contact = null;
        private Form parentForm = null;

        private int default_w = 546, default_h = 600;

        private Size pictureSize = new Size(50,50);

        public ContactResults( Form parentForm , Contact contact )
        {
            this.contact = contact;
            this.parentForm = parentForm;
            InitializeComponent();

            /**
             * We set up the width of the window. In the case that we have engine results to show we are going to change it
             * First width, then height
             * */
            this.Size = new System.Drawing.Size(this.default_w, this.default_h);

            this.setUpContactInformation();
        }

        private void setUpContactInformation() {
            this.txtFamilyName.Text = this.contact.familyName;
            this.txtFullName.Text = this.contact.fullName;
            this.txtGivenName.Text = this.contact.givenName;
            this.txtRequestBox.Text = this.contact.requestId;
            this.txtEmail.Text = this.contact.email;
            this.Text = "Contact Result - " + ( this.contact.likelihood * 100).ToString() + '%'.ToString();
            /**
             * After the main information, we want to add the rest of the stuff. The following
             * elements are different because they are not a fixed size, so they can be anything.
             * */
            this.setUpContactPhotos();
            this.SetUpSocialProfiles();
        }

        /**
         * This method will print all the photos of the user (if it has any)
         * */
        private void setUpContactPhotos() {
            PictureBox picture = null;
            foreach (  ContactPhoto photo in this.contact.photos.ToArray() ) {
                try
                {
                    picture = new PictureBox();
                    picture.Size = this.pictureSize; // There is a default size in a private variable for this class
                    picture.Cursor = Cursors.Hand;
                    picture.MouseClick += new MouseEventHandler((o, a) =>
                                Process.Start(photo.url)
                    );
                    picture.Load(photo.url);
                    picture.SizeMode = PictureBoxSizeMode.Zoom;
                    this.spacePhotos.Controls.Add(picture);
                }
                catch (Exception err) {
                    String message = "The URL could not be loaded: " + photo.url;
                    System.Console.WriteLine( message );
                    System.Console.WriteLine( err.Message );
                }
                
            }
        }


        private void SetUpSocialProfiles() {
            PictureBox picture = null;
            foreach (ContactSocialProfile profile in this.contact.social_profiles.ToArray())
            {
                try
                {
                    picture = new PictureBox();
                    picture.Size = this.pictureSize; // There is a default size in a private variable for this class
                    picture.Cursor = Cursors.Hand;

                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(picture, profile.typeId);

                    picture.MouseClick += new MouseEventHandler((o, a) =>
                                Process.Start(profile.url)
                    );

                    switch (profile.typeId) {
                        case "facebook":
                            picture.Image = Properties.Resources.facebook;
                            break;
                        case "twitter":
                            picture.Image = Properties.Resources.twitter;
                            break;
                        case "google":
                            picture.Image = Properties.Resources.google;
                            break;
                        case "googleplus":
                            picture.Image = Properties.Resources.google;
                            break;
                        case "github":
                            picture.Image = Properties.Resources.github;
                            break;
                        case "instagram":
                            picture.Image = Properties.Resources.instagram;
                            break;
                        case "linkedin":
                            picture.Image = Properties.Resources.linkedin;
                            break;
                        case "devianart":
                            picture.Image = Properties.Resources.deviantart;
                            break;
                        case "youtube":
                            picture.Image = Properties.Resources.youtube;
                            break;
                        default:
                            //We don't have the icon for this one or we don't knot what is the social profile yet.
                            picture.Image = Properties.Resources.defaultsocial;
                            break;
                    }
                    
                    picture.Refresh();
                    picture.SizeMode = PictureBoxSizeMode.Zoom;
                    this.spaceSocialProfiles.Controls.Add(picture);
                }
                catch (Exception err) {
                    System.Console.WriteLine(err.Message);
                }
                
            }
        }



        private void ContactResults_Load(object sender, EventArgs e)
        {

        }

        private void closeApp(object sender, FormClosedEventArgs e)
        {
            this.parentForm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //We put the waiting cursor and later we restore the original one.
            this.Cursor = Cursors.WaitCursor;
            MagicContact.FullContact fullContact = new MagicContact.FullContact();
            bool success = fullContact.saveHTML( this.contact.email );
            this.Cursor = Cursors.Default; //lets put the original cursor as its original state
            if (success)
            {
                System.Windows.Forms.MessageBox.Show("The file was saved" , "Saved" , MessageBoxButtons.OK , MessageBoxIcon.Information);
            }
            else {
                System.Windows.Forms.MessageBox.Show("Error saving the file","Error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
