// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Contracts
{
	using System.ServiceModel;
	using System.Threading.Tasks;

	using VahapYigit.Test.Core;

	[ServiceContract(Namespace = VahapYigit.Test.Core.Globals.Namespace)]
	public interface IService
	{
		[OperationContract]
		bool WakeUp(IUserContext userContext);

		[OperationContract(Name = "WakeUpAsync")]
		Task<bool> WakeUpAsync(IUserContext userContext);
	}
}
