using Neo.Cryptography.ECC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace Neo.SmartContract.Testing;

public abstract class Contract_Syscall_Out(Neo.SmartContract.Testing.SmartContractInitialize initialize) : Neo.SmartContract.Testing.SmartContract(initialize), IContractInfo
{
    #region Compiled data

    public static Neo.SmartContract.Manifest.ContractManifest Manifest => Neo.SmartContract.Manifest.ContractManifest.Parse(@"{""name"":""Contract_Syscall_Out"",""groups"":[],""features"":{},""supportedstandards"":[],""abi"":{""methods"":[{""name"":""testByteTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":0,""safe"":false},{""name"":""testSByteTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":39,""safe"":false},{""name"":""testShortTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":79,""safe"":false},{""name"":""testUShortTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":122,""safe"":false},{""name"":""testIntTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":163,""safe"":false},{""name"":""testUIntTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":212,""safe"":false},{""name"":""testLongTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":257,""safe"":false},{""name"":""testULongTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":318,""safe"":false},{""name"":""testBoolTryParse"",""parameters"":[{""name"":""s"",""type"":""String""}],""returntype"":""Array"",""offset"":375,""safe"":false},{""name"":""_initialize"",""parameters"":[],""returntype"":""Void"",""offset"":597,""safe"":false}],""events"":[]},""permissions"":[{""contract"":""0xacce6fd80d44e1796aa0c2c625e9e4e0ce39efc0"",""methods"":[""atoi""]}],""trusts"":[],""extra"":{""nef"":{""optimization"":""All""}}}");

    /// <summary>
    /// Optimization: "All"
    /// </summary>
    public static Neo.SmartContract.NefFile Nef => Neo.IO.Helper.AsSerializable<Neo.SmartContract.NefFile>(Convert.FromBase64String(@"TkVGM1Rlc3RpbmdFbmdpbmUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHA7znO4OTpJcbCoGp54UQN2G/OrARhdG9pAQABDwAA/VgCVwEBEGBYeFBFNwAAStgkDkoQAQABuyYGYAgiBEUJcMVKaM9KWM9AVwEBEGFZeFBFNwAAStgkD0oAgAGAALsmBmEIIgRFCXDFSmjPSlnPQFcBARBiWnhQRTcAAErYJBJKAQCAAgCAAAC7JgZiCCIERQlwxUpoz0paz0BXAQEQY1t4UEU3AABK2CQQShACAAABALsmBmMIIgRFCXDFSmjPSlvPQFcBARBkXHhQRTcAAErYJBhKAgAAAIADAAAAgAAAAAC7JgZkCCIERQlwxUpoz0pcz0BXAQEQZV14UEU3AABK2CQUShADAAAAAAEAAAC7JgZlCCIERQlwxUpoz0pdz0BXAQEQZl54UEU3AABK2CQkSgMAAAAAAAAAgAQAAAAAAAAAgAAAAAAAAAAAuyYGZggiBEUJcMVKaM9KXs9AVwEBEGcHXwd4UEU3AABK2CQdShAEAAAAAAAAAAABAAAAAAAAALsmB2cHCCIERQlwxUpoz0pfB89AVwEBEGcIXwh4UEVKDAR0cnVllyW1AAAASgwEVFJVRZclqAAAAEoMBFRydWWXJZsAAABKDAF0lyWRAAAASgwBVJclhwAAAEoMATGXJH1KDAN5ZXOXJHRKDANZRVOXJGtKDAF5lyRkSgwBWZckXUoMBWZhbHNllyRZSgwFRkFMU0WXJE5KDAVGYWxzZZckQ0oMAWaXJDxKDAFGlyQ1SgwBMJckLkoMAm5vlyQmSgwCTk+XJB5KDAFulyQXSgwBTpckEEUJZwgJIg5FCGcICCIHRQlnCAhwxUpoz0pfCM9AVglA4h///g=="));

    #endregion

    #region Unsafe methods

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGcIXwh4UEVKDHRydWWXJbUAAABKDFRSVUWXJagAAABKDFRydWWXJZsAAABKDHSXJZEAAABKDFSXJYcAAABKDDGXJH1KDHllc5ckdEoMWUVTlyRrSgx5lyRkSgxZlyRdSgxmYWxzZZckWUoMRkFMU0WXJE5KDEZhbHNllyRDSgxmlyQ8SgxGlyQ1SgwwlyQuSgxub5ckJkoMTk+XJB5KDG6XJBdKDE6XJBBFCWcICSIORQhnCAgiB0UJZwgIcMVKaM9KXwjPQA==
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD 08
    /// 06 : OpCode.LDSFLD 08
    /// 08 : OpCode.LDARG0
    /// 09 : OpCode.SWAP
    /// 0A : OpCode.DROP
    /// 0B : OpCode.DUP
    /// 0C : OpCode.PUSHDATA1 74727565
    /// 12 : OpCode.EQUAL
    /// 13 : OpCode.JMPIF_L B5000000
    /// 18 : OpCode.DUP
    /// 19 : OpCode.PUSHDATA1 54525545
    /// 1F : OpCode.EQUAL
    /// 20 : OpCode.JMPIF_L A8000000
    /// 25 : OpCode.DUP
    /// 26 : OpCode.PUSHDATA1 54727565
    /// 2C : OpCode.EQUAL
    /// 2D : OpCode.JMPIF_L 9B000000
    /// 32 : OpCode.DUP
    /// 33 : OpCode.PUSHDATA1 74
    /// 36 : OpCode.EQUAL
    /// 37 : OpCode.JMPIF_L 91000000
    /// 3C : OpCode.DUP
    /// 3D : OpCode.PUSHDATA1 54
    /// 40 : OpCode.EQUAL
    /// 41 : OpCode.JMPIF_L 87000000
    /// 46 : OpCode.DUP
    /// 47 : OpCode.PUSHDATA1 31
    /// 4A : OpCode.EQUAL
    /// 4B : OpCode.JMPIF 7D
    /// 4D : OpCode.DUP
    /// 4E : OpCode.PUSHDATA1 796573
    /// 53 : OpCode.EQUAL
    /// 54 : OpCode.JMPIF 74
    /// 56 : OpCode.DUP
    /// 57 : OpCode.PUSHDATA1 594553
    /// 5C : OpCode.EQUAL
    /// 5D : OpCode.JMPIF 6B
    /// 5F : OpCode.DUP
    /// 60 : OpCode.PUSHDATA1 79
    /// 63 : OpCode.EQUAL
    /// 64 : OpCode.JMPIF 64
    /// 66 : OpCode.DUP
    /// 67 : OpCode.PUSHDATA1 59
    /// 6A : OpCode.EQUAL
    /// 6B : OpCode.JMPIF 5D
    /// 6D : OpCode.DUP
    /// 6E : OpCode.PUSHDATA1 66616C7365
    /// 75 : OpCode.EQUAL
    /// 76 : OpCode.JMPIF 59
    /// 78 : OpCode.DUP
    /// 79 : OpCode.PUSHDATA1 46414C5345
    /// 80 : OpCode.EQUAL
    /// 81 : OpCode.JMPIF 4E
    /// 83 : OpCode.DUP
    /// 84 : OpCode.PUSHDATA1 46616C7365
    /// 8B : OpCode.EQUAL
    /// 8C : OpCode.JMPIF 43
    /// 8E : OpCode.DUP
    /// 8F : OpCode.PUSHDATA1 66
    /// 92 : OpCode.EQUAL
    /// 93 : OpCode.JMPIF 3C
    /// 95 : OpCode.DUP
    /// 96 : OpCode.PUSHDATA1 46
    /// 99 : OpCode.EQUAL
    /// 9A : OpCode.JMPIF 35
    /// 9C : OpCode.DUP
    /// 9D : OpCode.PUSHDATA1 30
    /// A0 : OpCode.EQUAL
    /// A1 : OpCode.JMPIF 2E
    /// A3 : OpCode.DUP
    /// A4 : OpCode.PUSHDATA1 6E6F
    /// A8 : OpCode.EQUAL
    /// A9 : OpCode.JMPIF 26
    /// AB : OpCode.DUP
    /// AC : OpCode.PUSHDATA1 4E4F
    /// B0 : OpCode.EQUAL
    /// B1 : OpCode.JMPIF 1E
    /// B3 : OpCode.DUP
    /// B4 : OpCode.PUSHDATA1 6E
    /// B7 : OpCode.EQUAL
    /// B8 : OpCode.JMPIF 17
    /// BA : OpCode.DUP
    /// BB : OpCode.PUSHDATA1 4E
    /// BE : OpCode.EQUAL
    /// BF : OpCode.JMPIF 10
    /// C1 : OpCode.DROP
    /// C2 : OpCode.PUSHF
    /// C3 : OpCode.STSFLD 08
    /// C5 : OpCode.PUSHF
    /// C6 : OpCode.JMP 0E
    /// C8 : OpCode.DROP
    /// C9 : OpCode.PUSHT
    /// CA : OpCode.STSFLD 08
    /// CC : OpCode.PUSHT
    /// CD : OpCode.JMP 07
    /// CF : OpCode.DROP
    /// D0 : OpCode.PUSHF
    /// D1 : OpCode.STSFLD 08
    /// D3 : OpCode.PUSHT
    /// D4 : OpCode.STLOC0
    /// D5 : OpCode.NEWSTRUCT0
    /// D6 : OpCode.DUP
    /// D7 : OpCode.LDLOC0
    /// D8 : OpCode.APPEND
    /// D9 : OpCode.DUP
    /// DA : OpCode.LDSFLD 08
    /// DC : OpCode.APPEND
    /// DD : OpCode.RET
    /// </remarks>
    [DisplayName("testBoolTryParse")]
    public abstract IList<object>? TestBoolTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGBYeFBFNwAAStgkDkoQAQABuyYGYAgiBEUJcMVKaM9KWM9A
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD0
    /// 05 : OpCode.LDSFLD0
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 0E
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSH0
    /// 12 : OpCode.PUSHINT16 0001
    /// 15 : OpCode.WITHIN
    /// 16 : OpCode.JMPIFNOT 06
    /// 18 : OpCode.STSFLD0
    /// 19 : OpCode.PUSHT
    /// 1A : OpCode.JMP 04
    /// 1C : OpCode.DROP
    /// 1D : OpCode.PUSHF
    /// 1E : OpCode.STLOC0
    /// 1F : OpCode.NEWSTRUCT0
    /// 20 : OpCode.DUP
    /// 21 : OpCode.LDLOC0
    /// 22 : OpCode.APPEND
    /// 23 : OpCode.DUP
    /// 24 : OpCode.LDSFLD0
    /// 25 : OpCode.APPEND
    /// 26 : OpCode.RET
    /// </remarks>
    [DisplayName("testByteTryParse")]
    public abstract IList<object>? TestByteTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGRceFBFNwAAStgkGEoCAAAAgAMAAACAAAAAALsmBmQIIgRFCXDFSmjPSlzPQA==
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD4
    /// 05 : OpCode.LDSFLD4
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 18
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSHINT32 00000080
    /// 16 : OpCode.PUSHINT64 0000008000000000
    /// 1F : OpCode.WITHIN
    /// 20 : OpCode.JMPIFNOT 06
    /// 22 : OpCode.STSFLD4
    /// 23 : OpCode.PUSHT
    /// 24 : OpCode.JMP 04
    /// 26 : OpCode.DROP
    /// 27 : OpCode.PUSHF
    /// 28 : OpCode.STLOC0
    /// 29 : OpCode.NEWSTRUCT0
    /// 2A : OpCode.DUP
    /// 2B : OpCode.LDLOC0
    /// 2C : OpCode.APPEND
    /// 2D : OpCode.DUP
    /// 2E : OpCode.LDSFLD4
    /// 2F : OpCode.APPEND
    /// 30 : OpCode.RET
    /// </remarks>
    [DisplayName("testIntTryParse")]
    public abstract IList<object>? TestIntTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGZeeFBFNwAAStgkJEoDAAAAAAAAAIAEAAAAAAAAAIAAAAAAAAAAALsmBmYIIgRFCXDFSmjPSl7PQA==
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD6
    /// 05 : OpCode.LDSFLD6
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 24
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSHINT64 0000000000000080
    /// 1A : OpCode.PUSHINT128 00000000000000800000000000000000
    /// 2B : OpCode.WITHIN
    /// 2C : OpCode.JMPIFNOT 06
    /// 2E : OpCode.STSFLD6
    /// 2F : OpCode.PUSHT
    /// 30 : OpCode.JMP 04
    /// 32 : OpCode.DROP
    /// 33 : OpCode.PUSHF
    /// 34 : OpCode.STLOC0
    /// 35 : OpCode.NEWSTRUCT0
    /// 36 : OpCode.DUP
    /// 37 : OpCode.LDLOC0
    /// 38 : OpCode.APPEND
    /// 39 : OpCode.DUP
    /// 3A : OpCode.LDSFLD6
    /// 3B : OpCode.APPEND
    /// 3C : OpCode.RET
    /// </remarks>
    [DisplayName("testLongTryParse")]
    public abstract IList<object>? TestLongTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGFZeFBFNwAAStgkD0oAgAGAALsmBmEIIgRFCXDFSmjPSlnPQA==
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD1
    /// 05 : OpCode.LDSFLD1
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 0F
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSHINT8 80
    /// 13 : OpCode.PUSHINT16 8000
    /// 16 : OpCode.WITHIN
    /// 17 : OpCode.JMPIFNOT 06
    /// 19 : OpCode.STSFLD1
    /// 1A : OpCode.PUSHT
    /// 1B : OpCode.JMP 04
    /// 1D : OpCode.DROP
    /// 1E : OpCode.PUSHF
    /// 1F : OpCode.STLOC0
    /// 20 : OpCode.NEWSTRUCT0
    /// 21 : OpCode.DUP
    /// 22 : OpCode.LDLOC0
    /// 23 : OpCode.APPEND
    /// 24 : OpCode.DUP
    /// 25 : OpCode.LDSFLD1
    /// 26 : OpCode.APPEND
    /// 27 : OpCode.RET
    /// </remarks>
    [DisplayName("testSByteTryParse")]
    public abstract IList<object>? TestSByteTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGJaeFBFNwAAStgkEkoBAIACAIAAALsmBmIIIgRFCXDFSmjPSlrPQA==
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD2
    /// 05 : OpCode.LDSFLD2
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 12
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSHINT16 0080
    /// 14 : OpCode.PUSHINT32 00800000
    /// 19 : OpCode.WITHIN
    /// 1A : OpCode.JMPIFNOT 06
    /// 1C : OpCode.STSFLD2
    /// 1D : OpCode.PUSHT
    /// 1E : OpCode.JMP 04
    /// 20 : OpCode.DROP
    /// 21 : OpCode.PUSHF
    /// 22 : OpCode.STLOC0
    /// 23 : OpCode.NEWSTRUCT0
    /// 24 : OpCode.DUP
    /// 25 : OpCode.LDLOC0
    /// 26 : OpCode.APPEND
    /// 27 : OpCode.DUP
    /// 28 : OpCode.LDSFLD2
    /// 29 : OpCode.APPEND
    /// 2A : OpCode.RET
    /// </remarks>
    [DisplayName("testShortTryParse")]
    public abstract IList<object>? TestShortTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGVdeFBFNwAAStgkFEoQAwAAAAABAAAAuyYGZQgiBEUJcMVKaM9KXc9A
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD5
    /// 05 : OpCode.LDSFLD5
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 14
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSH0
    /// 12 : OpCode.PUSHINT64 0000000001000000
    /// 1B : OpCode.WITHIN
    /// 1C : OpCode.JMPIFNOT 06
    /// 1E : OpCode.STSFLD5
    /// 1F : OpCode.PUSHT
    /// 20 : OpCode.JMP 04
    /// 22 : OpCode.DROP
    /// 23 : OpCode.PUSHF
    /// 24 : OpCode.STLOC0
    /// 25 : OpCode.NEWSTRUCT0
    /// 26 : OpCode.DUP
    /// 27 : OpCode.LDLOC0
    /// 28 : OpCode.APPEND
    /// 29 : OpCode.DUP
    /// 2A : OpCode.LDSFLD5
    /// 2B : OpCode.APPEND
    /// 2C : OpCode.RET
    /// </remarks>
    [DisplayName("testUIntTryParse")]
    public abstract IList<object>? TestUIntTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGcHXwd4UEU3AABK2CQdShAEAAAAAAAAAAABAAAAAAAAALsmB2cHCCIERQlwxUpoz0pfB89A
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD 07
    /// 06 : OpCode.LDSFLD 07
    /// 08 : OpCode.LDARG0
    /// 09 : OpCode.SWAP
    /// 0A : OpCode.DROP
    /// 0B : OpCode.CALLT 0000
    /// 0E : OpCode.DUP
    /// 0F : OpCode.ISNULL
    /// 10 : OpCode.JMPIF 1D
    /// 12 : OpCode.DUP
    /// 13 : OpCode.PUSH0
    /// 14 : OpCode.PUSHINT128 00000000000000000100000000000000
    /// 25 : OpCode.WITHIN
    /// 26 : OpCode.JMPIFNOT 07
    /// 28 : OpCode.STSFLD 07
    /// 2A : OpCode.PUSHT
    /// 2B : OpCode.JMP 04
    /// 2D : OpCode.DROP
    /// 2E : OpCode.PUSHF
    /// 2F : OpCode.STLOC0
    /// 30 : OpCode.NEWSTRUCT0
    /// 31 : OpCode.DUP
    /// 32 : OpCode.LDLOC0
    /// 33 : OpCode.APPEND
    /// 34 : OpCode.DUP
    /// 35 : OpCode.LDSFLD 07
    /// 37 : OpCode.APPEND
    /// 38 : OpCode.RET
    /// </remarks>
    [DisplayName("testULongTryParse")]
    public abstract IList<object>? TestULongTryParse(string? s);

    /// <summary>
    /// Unsafe method
    /// </summary>
    /// <remarks>
    /// Script: VwEBEGNbeFBFNwAAStgkEEoQAgAAAQC7JgZjCCIERQlwxUpoz0pbz0A=
    /// 00 : OpCode.INITSLOT 0101
    /// 03 : OpCode.PUSH0
    /// 04 : OpCode.STSFLD3
    /// 05 : OpCode.LDSFLD3
    /// 06 : OpCode.LDARG0
    /// 07 : OpCode.SWAP
    /// 08 : OpCode.DROP
    /// 09 : OpCode.CALLT 0000
    /// 0C : OpCode.DUP
    /// 0D : OpCode.ISNULL
    /// 0E : OpCode.JMPIF 10
    /// 10 : OpCode.DUP
    /// 11 : OpCode.PUSH0
    /// 12 : OpCode.PUSHINT32 00000100
    /// 17 : OpCode.WITHIN
    /// 18 : OpCode.JMPIFNOT 06
    /// 1A : OpCode.STSFLD3
    /// 1B : OpCode.PUSHT
    /// 1C : OpCode.JMP 04
    /// 1E : OpCode.DROP
    /// 1F : OpCode.PUSHF
    /// 20 : OpCode.STLOC0
    /// 21 : OpCode.NEWSTRUCT0
    /// 22 : OpCode.DUP
    /// 23 : OpCode.LDLOC0
    /// 24 : OpCode.APPEND
    /// 25 : OpCode.DUP
    /// 26 : OpCode.LDSFLD3
    /// 27 : OpCode.APPEND
    /// 28 : OpCode.RET
    /// </remarks>
    [DisplayName("testUShortTryParse")]
    public abstract IList<object>? TestUShortTryParse(string? s);

    #endregion
}
