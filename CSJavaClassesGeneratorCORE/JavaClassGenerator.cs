using Ac4yClassModule.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSJavaClassesGeneratorCORE
{
    class JavaClassGenerator
    {

        #region members

        public string TemplatePath { get; set; }
        public string TemplateSubPath { get; set; }
        public string OutputPath { get; set; }
        public string ProjectName { get; set; }

        public Ac4yClass Ac4yClass { get; set; }

        private const string TemplateExtension = ".csT";

        private const string Suffix = "BaseClass";

        private const string ClassCodeMask = "#classCode#";
        private const string SuffixMask = "#suffix#";
        private const string PackageNameMask = "#packageName#";
        private const string InheritanceMask = "#inheritance#";
        private const string PropertyTypeMask = "#type#";
        private const string PropertyNameMask = "#propertyName#";
        private const string PropertyInfoMask = "#propertyInfo#";

        private const string ClassCodeAsVariableMask = "#classCodeAsVariable#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = TemplatePath + TemplateSubPath + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            File.WriteAllText(outputPath + fileName + ".java", text);

        }

        public string GetNameWithLowerFirstLetter(String Code)
        {
            return
                char.ToLower(Code[0])
                + Code.Substring(1)
                ;

        } // GetNameWithLowerFirstLetter

        public string GetHead()
        {

            return ReadIntoString("Head")
                        .Replace(ClassCodeMask, Ac4yClass.Name)
                        .Replace(InheritanceMask, !Ac4yClass.Ancestor.Equals("Object") ? "extends " + Ac4yClass.Ancestor : "")
                        .Replace(PackageNameMask, ProjectName)
                        ;

        }

        public string GetFoot()
        {
            return
                ReadIntoString("Foot")
                        .Replace(ClassCodeMask, Ac4yClass.Name)
                        .Replace(SuffixMask, Suffix)
                        ;

        }

        public string GetMethods()
        {
            return
                ReadIntoString("Methods")
                ;
        }

        public string GetProperties(Ac4yProperty ac4yProperty)
        {
            return
                ReadIntoString("Properties")
                        .Replace(PropertyNameMask, ac4yProperty.Name)
                        .Replace(PropertyTypeMask, ac4yProperty.Type.Equals("Int32") ? "Integer" : ac4yProperty.Type)
                ;
        }

        public JavaClassGenerator Generate()
        {

            string result = null;

            result += GetHead();

            if(Ac4yClass.PropertyList.Count > 0)
            {
                for(int propertyNumber = 0; propertyNumber < Ac4yClass.PropertyList.Count; propertyNumber++)
                {
                    result += GetProperties(Ac4yClass.PropertyList[propertyNumber]);
                }
            }

            result += GetMethods();

            result += GetFoot();

            WriteOut(result, Ac4yClass.Name, OutputPath);

            return this;

        } // Generate

        public JavaClassGenerator Generate(Ac4yClass ac4yClass)
        {

            Ac4yClass = ac4yClass;

            return Generate();

        } // Generate

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }

} // JavaClassGenerator