﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Example2
{
    public partial class Form1 : Form
    {
        private SolidEdgeFramework.Application _application;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Toggle the label visible state.
            label1.Visible = !label1.Visible;

            // Get a reference to Solid Edge if we don't already have one.
            if (_application == null)
            {
                try
                {
                    // Attempt to connect to a running instace.
                    _application = (SolidEdgeFramework.Application)Marshal.GetActiveObject(SolidEdge.PROGID.Application);
                }
                catch
                {
                    // Start a new instance.
                    _application = new SolidEdgeFramework.Application();
                }
            }

            // Make sure Solid Edge is visible.
            _application.Visible = true;

            // See what AppDomain we're currently executing in.
            var currentAppDomain = AppDomain.CurrentDomain;

            // This will always be the default AppDomain at this point.
            var isDefaultAppDomain = currentAppDomain.IsDefaultAppDomain();

            // Create a separate AppDomain and execute our code.
            CreateSeparateAppDomainAndExecuteIsolatedTask(_application);

            // Toggle the label visible state.
            label1.Visible = !label1.Visible;
        }

        private void CreateSeparateAppDomainAndExecuteIsolatedTask(SolidEdgeFramework.Application application)
        {
            AppDomain interopDomain = null;

            try
            {
                var thread = new System.Threading.Thread(() =>
                {
                    // Create a custom AppDomain to do COM Interop.
                    interopDomain = AppDomain.CreateDomain("Interop Domain");

                    Type proxyType = typeof(InteropProxy);

                    // Create a new instance of InteropProxy in the isolated application domain.
                    InteropProxy interopProxy = interopDomain.CreateInstanceAndUnwrap(
                        proxyType.Assembly.FullName,
                        proxyType.FullName) as InteropProxy;

                    try
                    {
                        interopProxy.DoIsolatedTask(application);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

                // Important! Set thread apartment state to STA.
                thread.SetApartmentState(System.Threading.ApartmentState.STA);

                // Start the thread.
                thread.Start();

                // Wait for the thead to finish.
                thread.Join();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (interopDomain != null)
                {
                    // Unload the Interop AppDomain. This will automatically free up any COM references.
                    AppDomain.Unload(interopDomain);
                }
            }
        }
    }
}
