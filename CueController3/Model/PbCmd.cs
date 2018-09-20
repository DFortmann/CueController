using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CueController3.Model
{
    public class PbCmd
    {

        public MethodInfo method;
        public object[] args;
        public string name;

        private PbCmd(MethodInfo method, object[] args, string name)
        {
            this.method = method;
            this.args = args;
            this.name = name;
        }

        public static PbCmd GetPbCmd(string cmd)
        {
            try
            {
                string[] args = cmd.Trim().Split(',');
                if (args[0].Length < 5) return null;

                Type type = typeof(Auto);
                MethodInfo method = type.GetMethod(args[0].Substring(4), BindingFlags.Public | BindingFlags.Static);
                if (method == null) return null;

                ParameterInfo[] paraInfos = method.GetParameters();

                if (paraInfos.Length + 1 == args.Length)
                {
                    List<object> lo = new List<object>();

                    for (int i = 0; i < paraInfos.Length; ++i)
                    {
                        switch (paraInfos[i].ParameterType.Name)
                        {
                            case "Double":
                                lo.Add(double.Parse(args[i + 1]));
                                break;

                            case "String":
                                lo.Add(args[i + 1]);
                                break;

                            case "Int32":
                                lo.Add(int.Parse(args[i + 1]));
                                break;

                            case "Boolean":
                                lo.Add(bool.Parse(args[i + 1]));
                                break;

                            case "AutoError":
                                lo.Add(Enum.Parse(typeof(AutoError), args[i + 1]));
                                break;

                            case "SequenceTimeCodeMode":
                                lo.Add(Enum.Parse(typeof(SequenceTimeCodeMode), args[i + 1]));
                                break;

                            case "SequenceTimeCodeStopAction":
                                lo.Add(Enum.Parse(typeof(SequenceTimeCodeStopAction), args[i + 1]));
                                break;

                            case "TransportMode":
                                lo.Add(Enum.Parse(typeof(TransportMode), args[i + 1]));
                                break;

                            case "CuePlayMode":
                                lo.Add(Enum.Parse(typeof(CuePlayMode), args[i + 1]));
                                break;

                            case "TimeType":
                                lo.Add(Enum.Parse(typeof(TimeType), args[i + 1]));
                                break;

                            case "MediaOptionsType":
                                lo.Add(Enum.Parse(typeof(MediaOptionsType), args[i + 1]));
                                break;

                            case "MediaType1":
                                lo.Add(Enum.Parse(typeof(MediaType1), args[i + 1]));
                                break;

                            case "LayerType":
                                lo.Add(Enum.Parse(typeof(LayerType), args[i + 1]));
                                break;

                            case "ParamResourceType1":
                                lo.Add(Enum.Parse(typeof(ParamResourceType1), args[i + 1]));
                                break;

                            case "TreeItemType":
                                lo.Add(Enum.Parse(typeof(TreeItemType), args[i + 1]));
                                break;

                            default: return null;
                        }
                    }
                    return new PbCmd(method, lo.ToArray(), cmd);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
