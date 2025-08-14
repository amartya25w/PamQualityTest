using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArconRPA
{
    public class RoboticProcessAutomation
    {
        private dynamic JsonObjectInputData;


        #region SendAutomationData Method
        /// <summary>
        /// SendAutomationData - Use this method to send automation data to this class library for automation.
        /// </summary>
        /// <param name="jsonData">send json of string type</param>
        public void SendAutomationData(string jsonData, Process a, Action<Process> callback = null)
        {
            try
            {
                Logger.Log.Info("Json data received from client : " + jsonData);
                Logger.Log.Info("Before Invoking APIs");
                InvokeAPIs objInvokeAPIs = new InvokeAPIs();
                Logger.Log.Info("After Invoking APIs");
                Logger.Log.Info("before Parsing jsonData");
                //var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(jsonData);
                Logger.Log.Info("before Parsing jsonData");
                Logger.Log.Info("before checking Json key for blank and null");
                #region code changes formtputty start
                StepExecutor objStepExecutor = new StepExecutor();
                objStepExecutor.ExecuteSteps(data,a);
                callback?.Invoke(a);
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Log.Info("Inside Catch");
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion

        #region TestMethod
        /// <summary>
        /// SendDataForTesting - This code is written to test connector using json file
        /// </summary>
        /// <param name="jsonData"></param>
        //public void SendDataForTesting(dynamic jsonData)
        //{
        //    try
        //    {
        //        Logger.Log.Info("Inside SendDataForTesting");
        //        JsonObjectInputData = jsonData;

        //        Logger.Log.Info("Creating instance of StepExecutor");

        //        StepExecutor objStepExecutor = new StepExecutor();

        //        Logger.Log.Info("Creating instance of StepExecutor - Completed");
        //        Logger.Log.Info("Calling step Executor");

        //        objStepExecutor.ExecuteSteps(JsonObjectInputData);

        //        Logger.Log.Info("Calling step Executor - Completed");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Info("Inside Catch");
        //        Logger.Log.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //}
        #endregion
    }
}
