// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using VahapYigit.Test.Core;

	public class ApiControllerBase : System.Web.Http.ApiController
	{
		protected readonly IUserContext UserContext = ClientContext.Anonymous;
	}
}
