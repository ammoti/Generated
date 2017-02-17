// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;

	public static class EntityBaseExtensions
	{
		#region [ Ownership Extensions ]

		public static bool IsOwnershipEntity(this EntityBase entity)
		{
			if (entity == null)
			{
				ThrowException.ThrowArgumentNullException("entity");
			}

			return entity is IOwnershipEntity;
		}

		public static bool IsOwner(this EntityBase entity, long owner)
		{
			return (entity.IsOwnershipEntity()) ? ((IOwnershipEntity)entity).Owner == owner : false;
		}

		#endregion
	}
}
