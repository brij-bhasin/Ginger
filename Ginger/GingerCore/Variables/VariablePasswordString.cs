#region License
/*
Copyright © 2014-2018 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using System.Collections.Generic;
using Amdocs.Ginger.Repository;
using GingerCore.Actions;
using GingerCore.Properties;

namespace GingerCore.Variables
{
    public class VariablePasswordString : VariableBase 
    {
        public new static partial class Fields
        {
            public static string Password = "Password";
        }

        public VariablePasswordString()
        {
        }

        public override string VariableUIType
        {
            get { return GingerDicser.GetTermResValue(eTermResKey.Variable) + " Password String"; }
        }

        public override string VariableEditPage { get { return "VariablePasswordStringPage"; } }

        private string mPassword;
        [IsSerializedForLocalRepository]
        public string Password 
        {
            set { mPassword = value; Value = value; OnPropertyChanged("Formula"); }
            get { return mPassword; } 
        }
        
        public override string GetFormula()
        {
            return Password;
        }

        public override void ResetValue()
        {
            Value = Password; 
        }

        public override void GenerateAutoValue()
        { 
            //NA
        }

        public override System.Drawing.Image Image { get { return Resources.Const; } }
        public override string VariableType() { return "PasswordString"; }

        public override List<ActSetVariableValue.eSetValueOptions> GetSupportedOperations()
        {
            throw new System.NotImplementedException();
        }

        public override bool SupportSetValue { get { return false; } }
    }
}
