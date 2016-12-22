// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: ParamMsg.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbr = global::Google.Protobuf.Reflection;
namespace MotorXP.Protobuf.ParamMSg
{

    /// <summary>Holder for reflection information generated from ParamMsg.proto</summary>
    public static partial class ParamMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for ParamMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ParamMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5QYXJhbU1zZy5wcm90bxIJS29tTW9kdWxlIlgKCVJlZ1BhcmFtcxIOCgZ0",
            "YXJnZXQYASABKA0SDQoFcGFyYVAYAiABKAISDQoFcGFyYUkYAyABKAISDQoF",
            "cGFyYUQYBCABKAISDgoGdGd0VmFsGAUgASgCQhyqAhlNb3RvclhQLlByb3Rv",
            "YnVmLlBhcmFtTVNnYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::MotorXP.Protobuf.ParamMSg.RegParams), global::MotorXP.Protobuf.ParamMSg.RegParams.Parser, new[]{ "Target", "ParaP", "ParaI", "ParaD", "TgtVal" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// defining the parameter message
  /// </summary>
  public sealed partial class RegParams : pb::IMessage<RegParams> {
    private static readonly pb::MessageParser<RegParams> _parser = new pb::MessageParser<RegParams>(() => new RegParams());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RegParams> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::MotorXP.Protobuf.ParamMSg.ParamMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegParams() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegParams(RegParams other) : this() {
      target_ = other.target_;
      paraP_ = other.paraP_;
      paraI_ = other.paraI_;
      paraD_ = other.paraD_;
      tgtVal_ = other.tgtVal_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegParams Clone() {
      return new RegParams(this);
    }

    /// <summary>Field number for the "target" field.</summary>
    public const int TargetFieldNumber = 1;
    private uint target_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Target {
      get { return target_; }
      set {
        target_ = value;
      }
    }

    /// <summary>Field number for the "paraP" field.</summary>
    public const int ParaPFieldNumber = 2;
    private float paraP_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ParaP {
      get { return paraP_; }
      set {
        paraP_ = value;
      }
    }

    /// <summary>Field number for the "paraI" field.</summary>
    public const int ParaIFieldNumber = 3;
    private float paraI_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ParaI {
      get { return paraI_; }
      set {
        paraI_ = value;
      }
    }

    /// <summary>Field number for the "paraD" field.</summary>
    public const int ParaDFieldNumber = 4;
    private float paraD_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ParaD {
      get { return paraD_; }
      set {
        paraD_ = value;
      }
    }

    /// <summary>Field number for the "tgtVal" field.</summary>
    public const int TgtValFieldNumber = 5;
    private float tgtVal_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float TgtVal {
      get { return tgtVal_; }
      set {
        tgtVal_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RegParams);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RegParams other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Target != other.Target) return false;
      if (ParaP != other.ParaP) return false;
      if (ParaI != other.ParaI) return false;
      if (ParaD != other.ParaD) return false;
      if (TgtVal != other.TgtVal) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Target != 0) hash ^= Target.GetHashCode();
      if (ParaP != 0F) hash ^= ParaP.GetHashCode();
      if (ParaI != 0F) hash ^= ParaI.GetHashCode();
      if (ParaD != 0F) hash ^= ParaD.GetHashCode();
      if (TgtVal != 0F) hash ^= TgtVal.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Target != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(Target);
      }
      if (ParaP != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(ParaP);
      }
      if (ParaI != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(ParaI);
      }
      if (ParaD != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(ParaD);
      }
      if (TgtVal != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(TgtVal);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Target != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Target);
      }
      if (ParaP != 0F) {
        size += 1 + 4;
      }
      if (ParaI != 0F) {
        size += 1 + 4;
      }
      if (ParaD != 0F) {
        size += 1 + 4;
      }
      if (TgtVal != 0F) {
        size += 1 + 4;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RegParams other) {
      if (other == null) {
        return;
      }
      if (other.Target != 0) {
        Target = other.Target;
      }
      if (other.ParaP != 0F) {
        ParaP = other.ParaP;
      }
      if (other.ParaI != 0F) {
        ParaI = other.ParaI;
      }
      if (other.ParaD != 0F) {
        ParaD = other.ParaD;
      }
      if (other.TgtVal != 0F) {
        TgtVal = other.TgtVal;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Target = input.ReadUInt32();
            break;
          }
          case 21: {
            ParaP = input.ReadFloat();
            break;
          }
          case 29: {
            ParaI = input.ReadFloat();
            break;
          }
          case 37: {
            ParaD = input.ReadFloat();
            break;
          }
          case 45: {
            TgtVal = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
