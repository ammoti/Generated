// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Generic visitor interface for traversals.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Type of the visitor.
	/// </typeparam>
	public interface IAcceptor<T>
	{
		bool AcceptVisitor(IVisitor<T> visitor);
	}
}

/*
-----------------------------------------------------
Implementation Example
-----------------------------------------------------
  
namespace VahapYigit.Test.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VisitorPatternUnitTest
    {
        [TestMethod]
        public void VisitorPattern_Example()
        {
            Robot robot = new Robot();

            robot.AcceptVisitor(new Operator());    // level 1
            Console.WriteLine(robot.Diagnostic);

            robot.AcceptVisitor(new Technician());  // level 2
            Console.WriteLine(robot.Diagnostic);

            robot.AcceptVisitor(new Engineer());    // level 3
            Console.WriteLine(robot.Diagnostic);
        }

        class Robot : IAcceptor<Robot>
        {
            public string Diagnostic { get; set; }

            public bool AcceptVisitor(IVisitor<Robot> visitor)
            {
                return visitor.Visit(this);
            }
        }

        class Operator : IVisitor<Robot>
        {
            public bool Visit(Robot acceptor)
            {
                acceptor.Diagnostic += "Operator: \"The robot is broken and can't be started by the client.\"" + Environment.NewLine;

                return true;
            }
        }

        class Technician : IVisitor<Robot>
        {
            public bool Visit(Robot acceptor)
            {
                acceptor.Diagnostic += "Technician: \"The robot has got a software issue. Need support 3.\"" + Environment.NewLine;

                return true;
            }
        }

        class Engineer : IVisitor<Robot>
        {
            public bool Visit(Robot acceptor)
            {
                acceptor.Diagnostic += "Engineer: \"The robot's fireware has been upgraded. Issue fixed.\"" + Environment.NewLine;

                return true;
            }
        }
    }
}

*/