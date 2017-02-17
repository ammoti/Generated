// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface IEncryptorProvider
	{
		string Encrypt(string stringToEncrypt);

		byte[] Encrypt(byte[] dataToEncrypt);

		string Decrypt(string stringToDecrypt);

		byte[] Decrypt(byte[] dataToDecrypt);
	}
}
