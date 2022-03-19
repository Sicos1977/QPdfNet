using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using QPdfNet.InteropDotNet;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace QPdfNet.Interop
{
    /// <summary>
    ///     The exported qpdf api signatures.
    /// </summary>
    /// <remarks>
    ///     Please note this is only public for technical reasons (you can't proxy a internal interface).
    ///     It should be considered an internal interface and is NOT part of the public api and may have
    ///     breaking changes between releases.
    ///
    ///     API URL: https://github.com/tesseract-ocr/tesseract/blob/main/include/tesseract/capi.h
    /// </remarks>
    public interface ITessApiSignatures
    {
        #region General free function
        /// <summary>
        ///     Returns the current version
        /// </summary>
        /// <returns></returns>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessVersion")] 
        IntPtr GetVersion();

        /// <summary>
        ///     Deallocates the memory block occupied by text array
        /// </summary>
        /// <param name="arr"></param>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteTextArray")]
        void DeleteTextArray(IntPtr arr);

        /// <summary>
        ///     Deallocates the memory block occupied by integer array
        /// </summary>
        /// <param name="arr"></param>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteIntArray")] 
        void DeleteIntArray(IntPtr arr);
        #endregion
    }

    internal static class QPdfApi
    {
        #region Fields
        private static ITessApiSignatures native;
        #endregion

        #region Properties
        public static ITessApiSignatures Native
        {
            get
            {
                if (native == null)
                    Initialize();
                return native;
            }
        }
        #endregion

        #region BaseApiGetVersion
        public static string BaseApiGetVersion()
        {
            var versionHandle = Native.GetVersion();
            if (versionHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(versionHandle, Encoding.UTF8);
            return result;
        }
        #endregion

        #region BaseApiGetHOcrText
        public static string BaseApiGetHOcrText(HandleRef handle, int pageNum)
        {
            var txtHandle = Native.BaseApiGetHOcrTextInternal(handle, pageNum);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetAltoText
        public static string BaseApiGetAltoText(HandleRef handle, int pageNum)
        {
            var txtHandle = Native.BaseApiGetAltoTextInternal(handle, pageNum);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetTsvText
        public static string BaseApiGetTsvText(HandleRef handle, int pageNum)
        {
            var txtHandle = Native.BaseApiGetTsvTextInternal(handle, pageNum);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetBoxText
        public static string BaseApiGetBoxText(HandleRef handle, int pageNum)
        {
            var txtHandle = Native.BaseApiGetBoxTextInternal(handle, pageNum);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetLSTMBoxText
        public static string BaseApiGetLSTMBoxText(HandleRef handle, int pageNum)
        {
            var txtHandle = Native.BaseApiGetLstmBoxTextInternal(handle, pageNum);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetWordStrBoxText
        public static string BaseApiGetWordStrBoxText(HandleRef handle, int pageNum)
        {
            var txtHandle = Native.BaseApiGetWordStrBoxTextInternal(handle, pageNum);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetUNLVText
        public static string BaseApiGetUnlvText(HandleRef handle)
        {
            var txtHandle = Native.BaseApiGetUnlvTextInternal(handle);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiGetStringVariable
        public static string BaseApiGetStringVariable(HandleRef handle, string name)
        {
            var resultHandle = Native.BaseApiGetStringVariableInternal(handle, name);
            return resultHandle != IntPtr.Zero ? MarshalHelper.PtrToString(resultHandle, Encoding.UTF8) : null;
        }
        #endregion

        #region BaseApiGetUTF8Text
        public static string BaseApiGetUTF8Text(HandleRef handle)
        {
            var txtHandle = Native.BaseApiGetUTF8TextInternal(handle);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region BaseApiInit
        public static int BaseApiInit(HandleRef handle, string datapath, string language, int mode,
            IEnumerable<string> configFiles, IDictionary<string, object> initialValues, bool setOnlyNonDebugParams)
        {
            Guard.Require("handle", handle.Handle != IntPtr.Zero, "Handle for BaseApi, created through BaseApiCreate is required");
            Guard.RequireNotNullOrEmpty("language", language);
            Guard.RequireNotNull("configFiles", configFiles);
            Guard.RequireNotNull("initialValues", initialValues);

            var configFilesArray = new List<string>(configFiles).ToArray();
            var varNames = new string[initialValues.Count];
            var varValues = new string[initialValues.Count];
            var i = 0;

            foreach (var pair in initialValues)
            {
                Guard.Require("initialValues", !string.IsNullOrEmpty(pair.Key), "Variable must have a name");
                Guard.Require("initialValues", pair.Value != null, "Variable '{0}': The type '{1}' is not supported", pair.Key, pair.Value?.GetType());
                
                varNames[i] = pair.Key;

                if (TessConvert.TryToString(pair.Value, out var varValue))
                    varValues[i] = varValue;
                else
                    throw new ArgumentException(
                        $"Variable '{pair.Key}': The type '{pair.Value?.GetType()}' is not supported",
                        nameof(initialValues));
                i++;
            }

            return Native.BaseApiInit4(handle, datapath, language, mode,
                configFilesArray, configFilesArray.Length,
                varNames, varValues, new UIntPtr((uint)varNames.Length), setOnlyNonDebugParams);
        }
        #endregion

        #region BaseApiSetDebugVariable
        public static int BaseApiSetDebugVariable(HandleRef handle, string name, string value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = MarshalHelper.StringToPtr(value, Encoding.UTF8);
                return Native.BaseApiSetDebugVariable(handle, name, valuePtr);
            }
            finally
            {
                if (valuePtr != IntPtr.Zero) Marshal.FreeHGlobal(valuePtr);
            }
        }
        #endregion

        #region BaseApiSetVariable
        public static int BaseApiSetVariable(HandleRef handle, string name, string value)
        {
            var valuePtr = IntPtr.Zero;

            try
            {
                valuePtr = MarshalHelper.StringToPtr(value, Encoding.UTF8);
                return Native.BaseApiSetVariable(handle, name, valuePtr);
            }
            finally
            {
                if (valuePtr != IntPtr.Zero) Marshal.FreeHGlobal(valuePtr);
            }
        }
        #endregion

        #region Initialize
        public static void Initialize()
        {
            if (native != null) return;
            LeptonicaApi.Initialize();
            Helper.SetPath();
            native = InteropRuntimeImplementer.CreateInstance<ITessApiSignatures>();
        }
        #endregion

        #region ResultIteratorWordRecognitionLanguage
        public static string ResultIteratorWordRecognitionLanguage(HandleRef handle)
        {
            var txtHandle = Native.ResultIteratorWordRecognitionLanguageInternal(handle);

            return txtHandle != IntPtr.Zero
                ? MarshalHelper.PtrToString(txtHandle, Encoding.UTF8)
                : null;
        }
        #endregion

        #region ResultIteratorGetUTF8Text
        public static string ResultIteratorGetUTF8Text(HandleRef handle, PageIteratorLevel level)
        {
            var txtHandle = Native.ResultIteratorGetUTF8TextInternal(handle, level);
            if (txtHandle == IntPtr.Zero) return null;
            var result = MarshalHelper.PtrToString(txtHandle, Encoding.UTF8);
            Native.DeleteText(txtHandle);
            return result;
        }
        #endregion

        #region ChoiceIteratorGetUTF8Text
        /// <summary>
        ///     Returns the null terminated UTF-8 encoded text string for the current choice
        /// </summary>
        /// <remarks>
        ///     NOTE: Unlike LTRResultIterator::GetUTF8Text, the return points to an
        ///     internal structure and should NOT be delete[]ed to free after use.
        /// </remarks>
        /// <param name="choiceIteratorHandle"></param>
        /// <returns>string</returns>
        internal static string ChoiceIteratorGetUTF8Text(HandleRef choiceIteratorHandle)
        {
            Guard.Require("choiceIteratorHandle", choiceIteratorHandle.Handle != IntPtr.Zero, "ChoiceIterator Handle cannot be a null IntPtr and is required");
            var txtChoiceHandle = Native.ChoiceIteratorGetUTF8TextInternal(choiceIteratorHandle);
            return MarshalHelper.PtrToString(txtChoiceHandle, Encoding.UTF8);
        }
        #endregion
    }
}