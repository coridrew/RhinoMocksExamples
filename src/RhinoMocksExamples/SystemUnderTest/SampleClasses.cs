using System;
using System.Collections.Generic;
using System.Linq;

namespace RhinoMocksExamples.SystemUnderTest
{
    public interface ISampleInterface
    {
        string Property { get; set; }
        void VoidMethod();
        int MethodThatReturnsInteger(string s);
        object MethodThatReturnsObject(int i);
        void MethodWithTwoParameters(string first, string second);
        void MethodWithEnumerable(IEnumerable<int> numbers);
    }

    public class InteractingClass
    {
        public void CallTheInterface(ISampleInterface sampleInterface)
        {
            sampleInterface.VoidMethod();
        }

        public int CallTheInterfaceTwice(ISampleInterface sampleInterface)
        {
            return sampleInterface.MethodThatReturnsInteger("foo")
                   + sampleInterface.MethodThatReturnsInteger("bar");
        }

        public void IgnoreTheInterface(ISampleInterface sampleInterface) { }

        public int AddSevenToTheInterface(ISampleInterface sampleInterface, string extraParameter)
        {
            return sampleInterface.MethodThatReturnsInteger(extraParameter) + 7;
        }

        public int AddSevenToTheInterface(ISampleInterface sampleInterface)
        {
            return sampleInterface.MethodThatReturnsInteger(sampleInterface.Property) + 7;
        }

        public void SetPropertyOnTheInterface(ISampleInterface sampleInterface, string newValue)
        {
            sampleInterface.Property = newValue;
        }

        public void SendTheNumberList(ISampleInterface sampleInterface, int endingNumber)
        {
            var numbers = new List<int>();
            int u = 0;
            int d = endingNumber;
            while (d - u > 0)
            {
                numbers.Add(u);
                numbers.Add(d);
                u++;
                d--;
            }
            if (u == d)
            {
                numbers.Add(u);
            }
            sampleInterface.MethodWithEnumerable(numbers);
        }
    }
}