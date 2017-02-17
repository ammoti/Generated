// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Management;
	using System.Threading.Tasks;

	public interface IFingerPrintProvider
	{
		string GetValue(bool withGuidValue = false);
	}

	public class FingerPrintProvider : IFingerPrintProvider
	{
		#region [ Members ]

		private static string _fingerPrint = null;
		private static string _guidFingerPrint = null;

		#endregion

		#region [ IFingerPrintProvider Implementation ]

		public string GetValue(bool withGuidValue = false)
		{
			if (string.IsNullOrEmpty(_fingerPrint))
			{
				string cpuId = null, biosId = null, motherBoardId = null, hardDiskId = null, videoId = null, macId = null;

				Parallel.Invoke(
					() => { cpuId = GetCpuId(); },
					() => { biosId = GetBiosId(); },
					() => { motherBoardId = GetMotherBoardId(); },
					() => { hardDiskId = GetHardDiskId(); },
					() => { videoId = GetVideoId(); },
					() => { macId = GetMacId(); }
				);

				_fingerPrint = string.Format("{{CPU: {0}}} {{BIOS: {1}}} {{BASE: {2}}} {{HD: {3}}} {{VIDEO: {4}}} {{MAC: {5}}}",
					cpuId, biosId, motherBoardId, hardDiskId, videoId, macId);
			}

			_guidFingerPrint = _fingerPrint.ToGuid().ToString();

			return (withGuidValue) ? _guidFingerPrint : _fingerPrint;
		}

		#endregion

		#region [ Private Methods ]

		public static string GetIdentifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
		{
			string value = string.Empty;

			ManagementObjectCollection moCollection = new ManagementClass(wmiClass).GetInstances();
			foreach (ManagementObject mo in moCollection)
			{
				if (mo[wmiMustBeTrue].ToString() == "True")
				{
					if (string.IsNullOrEmpty(value))
					{
						try
						{
							value = mo[wmiProperty].ToString();
							break;
						}
						catch
						{
						}
					}
				}
			}

			return value;
		}

		public static string GetIdentifier(string wmiClass, string wmiProperty)
		{
			string value = string.Empty;

			ManagementObjectCollection moCollection = new ManagementClass(wmiClass).GetInstances();
			foreach (ManagementObject mo in moCollection)
			{
				if (string.IsNullOrEmpty(value))
				{
					try
					{
						value = mo[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}

			return value;
		}

		private static string GetCpuId()
		{
			string retVal = GetIdentifier("Win32_Processor", "UniqueId");
			if (retVal == string.Empty)
			{
				retVal = GetIdentifier("Win32_Processor", "ProcessorId");
				if (retVal == string.Empty)
				{
					retVal = GetIdentifier("Win32_Processor", "Name");
					if (retVal == string.Empty)
					{
						retVal = GetIdentifier("Win32_Processor", "Manufacturer");
					}

					retVal += GetIdentifier("Win32_Processor", "MaxClockSpeed");
				}
			}

			return retVal;
		}

		private static string GetBiosId()
		{
			return GetIdentifier("Win32_BIOS", "Manufacturer") +
				   GetIdentifier("Win32_BIOS", "SMBIOSBIOSVersion") +
				   GetIdentifier("Win32_BIOS", "IdentificationCode") +
				   GetIdentifier("Win32_BIOS", "SerialNumber") +
				   GetIdentifier("Win32_BIOS", "ReleaseDate") +
				   GetIdentifier("Win32_BIOS", "Version");
		}

		private static string GetHardDiskId()
		{
			return GetIdentifier("Win32_DiskDrive", "Model") +
				   GetIdentifier("Win32_DiskDrive", "Manufacturer") +
				   GetIdentifier("Win32_DiskDrive", "Signature") +
				   GetIdentifier("Win32_DiskDrive", "TotalHeads");
		}

		private static string GetMotherBoardId()
		{
			return GetIdentifier("Win32_BaseBoard", "Model") +
				   GetIdentifier("Win32_BaseBoard", "Manufacturer") +
				   GetIdentifier("Win32_BaseBoard", "Name") +
				   GetIdentifier("Win32_BaseBoard", "SerialNumber");
		}

		private static string GetVideoId()
		{
			return GetIdentifier("Win32_VideoController", "DriverVersion") +
				   GetIdentifier("Win32_VideoController", "Name");
		}

		private static string GetMacId()
		{
			return GetIdentifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
		}

		#endregion
	}
}