using System;
using System.Collections.Generic;

namespace RhinoMocksExamples.SystemUnderTest
{
    public interface ISampleInterface
    {
        string Property { get; set; }
        void VoidMethod();
        int MethodThatReturnsInteger(string s);
        object MethodThatReturnsObject(int i);
        void MethodWithOutParameter(out int i);
        void MethodWithRefParameter(ref string i);

        event EventHandler SomeEvent;
    }

    public class InteractingClass
    {
        public void CallTheInterface(ISampleInterface sampleInterface)
        {
            sampleInterface.VoidMethod();
        }

        public void IgnoreTheInterface(ISampleInterface sampleInterface)
        {
        }

        public int AddWithTheInterface(ISampleInterface sampleInterface, string extraParameter)
        {
            return sampleInterface.MethodThatReturnsInteger(extraParameter) + 7;
        }

        public int AddWithTheInterface(ISampleInterface sampleInterface)
        {
            return sampleInterface.MethodThatReturnsInteger(sampleInterface.Property) + 7;
        }
    }

    public class SampleClass
    {
        private string _nonVirtualProperty;

        private string _virtualProperty;

        public SampleClass()
        {
        }

        public SampleClass(string value)
        {
            SetByConstructor = value;
        }

        public string NonVirtualProperty
        {
            get { return _nonVirtualProperty; }
            set
            {
                _nonVirtualProperty = value;
                NonVirtualPropertyWasSet = true;
            }
        }

        public virtual string VirtualProperty
        {
            get { return _virtualProperty; }
            set
            {
                _virtualProperty = value;
                VirtualPropertyWasSet = true;
            }
        }

        public string SetByConstructor { get; private set; }
        public bool NonVirtualPropertyWasSet { get; set; }
        public bool VirtualPropertyWasSet { get; set; }
        public bool VoidMethodWasCalled { get; set; }
        public bool VirtualMethodWasCalled { get; set; }
        public bool NonVirtualMethodWasCalled { get; set; }

        public event EventHandler SomeEvent;
        public virtual event EventHandler SomeVirtualEvent;

        public void VoidMethod()
        {
            VoidMethodWasCalled = true;
        }

        public virtual int VirtualMethod(string s)
        {
            VirtualMethodWasCalled = true;
            return s.Length;
        }

        public IList<int> NonVirtualMethod(int i)
        {
            NonVirtualMethodWasCalled = true;
            return new List<int> {i};
        }

        public void FireSomeVirtualEvent()
        {
            if (SomeVirtualEvent != null)
                SomeVirtualEvent(this, EventArgs.Empty);
        }
    }
}