using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Animal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private delegate void AnimalSay(object sender);
        private event AnimalSay Say;

        public MainPage()
        {  
            this.InitializeComponent();
        }

        interface Animal
        {
            void say(object sender);
        }

        class cat : Animal
        {
            TextBlock word;

            public cat(TextBlock w)
            {
                this.word = w;
            }
            public void say(object sender)
            {
                this.word.Text += "Cat: I am a Cat" + "\n";
            }
        }

        class dog : Animal
        {
            TextBlock word;

            public dog(TextBlock w)
            {
                this.word = w;
            }
            public void say(object sender)
            {
                this.word.Text += "Dog: I am a Dog" + "\n";
            }
        }

        class pig : Animal
        {
            TextBlock word;

            public pig(TextBlock w)
            {
                this.word = w;
            }
            public void say(object sender)
            {
                this.word.Text += "Pig: I am a Pig" + "\n";
            }
        }

        private cat c;
        private dog d;
        private pig p;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            c = new cat(textBlock);
            d = new dog(textBlock);
            p = new pig(textBlock);
            Random ran = new Random();
            int RandKey = ran.Next(1, 4);
            switch(RandKey)
            {
                case 1:
                    Say += new AnimalSay(c.say);
                    break;
                case 2:
                    Say += new AnimalSay(d.say);
                    break;
                case 3:
                    Say += new AnimalSay(p.say);
                    break;
            }
            Say(this);
            Say = null;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            String theAnimal = textBox.Text;
            switch(theAnimal)
            {
                case "cat":
                    Say += new AnimalSay(c.say);
                    break;
                case "dog":
                    Say += new AnimalSay(d.say);
                    break;
                case "pig":
                    Say += new AnimalSay(p.say);
                    break;
                default:
                    break;
            }
            Say(this);
            Say = null;
            textBox.Text = "";
        }
    }
}
