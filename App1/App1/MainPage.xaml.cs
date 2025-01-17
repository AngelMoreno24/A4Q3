﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //Button event for random string generation
        private async void Button_Clicked(object sender, EventArgs e)
        {

            //call encryption service
            HttpClient client = new HttpClient();

            //result stores the output from calling the service and check is used for checking if the user input is an integer
            string result;
            int check;

            //checks if the input is an integer
            if (int.TryParse(stringSize.Text, out check))
            {

                //calls the service and stores the output into result
                try
                {
                    var response = await client.GetAsync(
                    "https://venus.sod.asu.edu/WSRepository/Services/RandomString/Service.svc/GetRandomString/" + stringSize.Text);
                    response.EnsureSuccessStatusCode();
                    result = (await response.Content.ReadAsStringAsync()).Replace(@"""", "");
                }
                catch (HttpRequestException ex)
                {
                    result = ex.ToString();
                }

                //while statement will look for the index after the '>' char
                int i = 0;
                while (result[i] != '>')
                {
                    i++;
                }
                i++;
                
                //output stores the string generated by the service while leaving the unnecessary serialization
                string output = "";

                //for statement stores the output string from calling the service
                for (int q=0; q < int.Parse(stringSize.Text); q++)
                {
                    output += result[i + q];
                }

                //displays the output on the GUI page
                stringOutput.Text = "Generated random password: " + output;

                //pushed the password to the encryption textbox for convenience
                EncryptEntry.Text = output;

            }

        }

        //Button event for the encryption section
        private async void Button_Clicked2(object sender, EventArgs e)
        {
            
            HttpClient client = new HttpClient();

            //stores the result of the Encryption service
            string result;

            //calls the Encryption service and stores the output into result
            try
            {
                var response = await client.GetAsync(
                "https://venus.sod.asu.edu/WSRepository/Services/EncryptionRest/Service.svc/Encrypt?text=" + EncryptEntry.Text);
                response.EnsureSuccessStatusCode();
                result = (await response.Content.ReadAsStringAsync()).Replace(@"""", "");
            }
            catch (HttpRequestException ex)
            {
                result = ex.ToString();
            }

            //displays the encrypted string onto the GUI page
            encryptOutput.Text = "Encrypted password: " + result;
        }
    }
}