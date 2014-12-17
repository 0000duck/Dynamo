﻿using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using Dynamo.Models;
using Dynamo.Nodes;
using ProtoCore.AST.AssociativeAST;
using DSCoreNodesUI.Properties;

namespace DSCore
{
    [NodeName(/*NXLT*/"List.Map")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListMapDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class Map : NodeModel
    {
        public Map(WorkspaceModel workspace) : base(workspace)
        {
            InPortData.Add(new PortData(/*NXLT*/"list", Resources.MapPortDataListToolTip));
            InPortData.Add(new PortData(/*NXLT*/"f(x)", Resources.MapPortDataFxToolTip));

            OutPortData.Add(new PortData(/*NXLT*/"mapped", Resources.MapPortDataResultToolTip));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    IsPartiallyApplied
                        ? AstFactory.BuildFunctionObject(
                            "__Map",
                            2,
                            new[] { 0, 1 }.Where(HasConnectedInput).Select(x => 1 - x),
                            Enumerable.Reverse(inputAstNodes).ToList())
                        : AstFactory.BuildFunctionCall("__Map", Enumerable.Reverse(inputAstNodes).ToList()))
            };
        }
    }

    public abstract class CombinatorNode : VariableInputNode
    {
        private readonly int minPorts;

        protected CombinatorNode(WorkspaceModel workspace) : this(workspace, 3)
        {
            InPortData.Add(new PortData(/*NXLT*/"comb", Resources.CombinatorPortDataCombToolTip));
            InPortData.Add(new PortData(/*NXLT*/"list1", Resources.PortDataList1ToolTip));
            InPortData.Add(new PortData(/*NXLT*/"list2", Resources.PortDataList2ToolTip));

            OutPortData.Add(new PortData(/*NXLT*/"combined", Resources.CombinatorPortDataResultToolTip));

            RegisterAllPorts();
        }

        protected CombinatorNode(WorkspaceModel workspace, int minPorts) : base(workspace)
        {
            this.minPorts = minPorts;
        }

        protected override string GetInputName(int index)
        {
            return "list" + index;
        }

        protected override string GetInputTooltip(int index)
        {
            return "List" + index;
        }

        protected override void RemoveInput()
        {
            if (InPortData.Count > minPorts)
                base.RemoveInput();
        }
    }

    [NodeName(/*NXLT*/"List.Combine")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListCombineDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class Combine : CombinatorNode
    {
        public Combine(WorkspaceModel workspace) : base(workspace) { }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__Combine",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList())
                        }))
            };
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    [NodeName(/*NXLT*/"List.ForEach")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListForEachDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class ForEach : CombinatorNode
    {
        public ForEach(WorkspaceModel workspace) : base(workspace, 2) { }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__ForEach",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList())
                        }))
            };
        }
    }

    //MAGN-3382 [IsVisibleInDynamoLibrary(false)]
    [NodeName(/*NXLT*/"List.LaceShortest")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListLaceShortestDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class LaceShortest : CombinatorNode
    {
        public LaceShortest(WorkspaceModel workspace) : base(workspace) { }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__LaceShortest",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList())
                        }))
            };
        }
    }

    //MAGN-3382 [IsVisibleInDynamoLibrary(false)]
    [NodeName(/*NXLT*/"List.LaceLongest")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListLaceLongestDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class LaceLongest : CombinatorNode
    {
        public LaceLongest(WorkspaceModel workspace) : base(workspace) { }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__LaceLongest",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList())
                        }))
            };
        }
    }

    ///<search>cross</search>
    //MAGN-3382 [IsVisibleInDynamoLibrary(false)]
    [NodeName(/*NXLT*/"List.CartesianProduct")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListCartesianProductDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class CartesianProduct : CombinatorNode
    {
        public CartesianProduct(WorkspaceModel workspace) : base(workspace) { }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__CartesianProduct",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList())
                        }))
            };
        }
    }

    /*
    [NodeName("True For Any")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_QUERY)]
    [NodeDescription("Tests to see if any elements in a sequence satisfy the given predicate.")]
    [IsDesignScriptCompatible]
    public class TrueForAny : NodeModel
    {
        public TrueForAny()
        {
            InPortData.Add(new PortData("list", "The list to test."));
            InPortData.Add(new PortData("p(x)", "The predicate used to test elements"));

            OutPortData.Add(new PortData("any?", "Whether or not any elements satisfy the given predicate."));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(
            List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "TrueForAny",
                        (inputAstNodes as IEnumerable<AssociativeNode>).Reverse().ToList()))
            };
        }
    }

    [NodeName("True For All")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_QUERY)]
    [NodeDescription("Tests to see if all elements in a sequence satisfy the given predicate.")]
    [IsDesignScriptCompatible]
    public class TrueForAll : NodeModel
    {
        public TrueForAll()
        {
            InPortData.Add(new PortData("list", "The list to test."));
            InPortData.Add(new PortData("p(x)", "The predicate used to test items"));

            OutPortData.Add(new PortData("all?", "Whether or not all items satisfy the given predicate."));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(
            List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "TrueForAll",
                        (inputAstNodes as IEnumerable<AssociativeNode>).Reverse().ToList()))
            };
        }
    }
    */

    [NodeName(/*NXLT*/"List.Reduce")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListReduceDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class Reduce : VariableInputNode
    {
        private readonly PortData reductorPort;

        public Reduce(WorkspaceModel workspaceModel) : base(workspaceModel)
        {
            InPortData.Add(new PortData(/*NXLT*/"reductor", Resources.ReducePortDataReductorToolTip));
            InPortData.Add(new PortData(/*NXLT*/"seed", Resources.ReducePortDataSeedToolTip));
            InPortData.Add(new PortData(/*NXLT*/"list1", Resources.PortDataList1ToolTip));

            OutPortData.Add(new PortData(/*NXLT*/"reduced", Resources.ReducePortDataResultToolTip));

            RegisterAllPorts();
        }

        protected override void RemoveInput()
        {
            if (InPortData.Count > 3)
            {
                base.RemoveInput();
                //UpdateReductorPort();
            }
        }

        protected override void AddInput()
        {
            base.AddInput();
            //UpdateReductorPort();
        }

        private void UpdateReductorPort()
        {
            if (InPortData.Count > 6) 
                reductorPort.NickName = "f(x1, x2, ... xN, a)";
            else
            {
                if (InPortData.Count == 3) 
                    reductorPort.NickName = "f(x, a)";
                else
                {
                    reductorPort.NickName = "f("
                        + string.Join(
                            ", ",
                            Enumerable.Range(0, InPortData.Count - 2).Select(x => "x" + (x + 1)))
                        + ", a)";
                }
            }
            RegisterAllPorts();
        }

        protected override string GetInputName(int index)
        {
            return "list" + index;
        }

        protected override string GetInputTooltip(int index)
        {
            return "List" + index;
        }

        protected override int GetInputIndex()
        {
            return InPortData.Count - 1;
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__Reduce",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            inputAstNodes[1],
                            AstFactory.BuildExprList(inputAstNodes.Skip(2).ToList())
                        }))
            };
        }
    }

    [NodeName(/*NXLT*/"List.Scan")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListScanDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class ScanList : VariableInputNode
    {
        private readonly PortData reductorPort;

        public ScanList(WorkspaceModel workspace) : base(workspace)
        {
            InPortData.Add(new PortData(/*NXLT*/"reductor", Resources.ScanPortDataReductorToolTip));
            InPortData.Add(new PortData(/*NXLT*/"seed", Resources.ScanPortDataSeedToolTip));
            InPortData.Add(new PortData(/*NXLT*/"list1", Resources.PortDataList1ToolTip));

            OutPortData.Add(new PortData(/*NXLT*/"scanned", Resources.ScanPortDataResultToolTip));

            RegisterAllPorts();
        }

        protected override void RemoveInput()
        {
            if (InPortData.Count > 3)
            {
                base.RemoveInput();
                //UpdateReductorPort();
            }
        }

        protected override void AddInput()
        {
            base.AddInput();
            //UpdateReductorPort();
        }

        private void UpdateReductorPort()
        {
            if (InPortData.Count > 6)
                reductorPort.NickName = "f(x1, x2, ... xN, a)";
            else
            {
                if (InPortData.Count == 3)
                    reductorPort.NickName = "f(x, a)";
                else
                {
                    reductorPort.NickName = "f("
                        + string.Join(
                            ", ",
                            Enumerable.Range(0, InPortData.Count - 2).Select(x => "x" + (x + 1)))
                        + ", a)";
                }
            }
            RegisterAllPorts();
        }

        protected override string GetInputName(int index)
        {
            return "list" + index;
        }

        protected override string GetInputTooltip(int index)
        {
            return "List" + index;
        }

        protected override int GetInputIndex()
        {
            return InPortData.Count - 1;
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall(
                        "__Scan",
                        new List<AssociativeNode>
                        {
                            inputAstNodes[0],
                            inputAstNodes[1],
                            AstFactory.BuildExprList(inputAstNodes.Skip(2).ToList())
                        }))
            };
        }
    }

    [NodeName(/*NXLT*/"List.Filter")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ListFilterDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class Filter : NodeModel
    {
        public Filter(WorkspaceModel workspace) : base(workspace)
        {
            InPortData.Add(new PortData(/*NXLT*/"list", Resources.FilterPortDataListToolTip));
            InPortData.Add(new PortData(/*NXLT*/"condition", Resources.FilterPortDataConditionToolTip));

            OutPortData.Add(new PortData(/*NXLT*/"in", Resources.FilterPortDataResultInToolTip));
            OutPortData.Add(new PortData(/*NXLT*/"out", Resources.FilterPortDataResultOutToolTip));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(
            List<AssociativeNode> inputAstNodes)
        {
            var packedId = "__temp" + GUID.ToString().Replace("-", "");
            return new[]
            {
                AstFactory.BuildAssignment(
                    AstFactory.BuildIdentifier(packedId),
                    AstFactory.BuildFunctionCall("__Filter", inputAstNodes)),
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    new IdentifierNode(packedId)
                    {
                        ArrayDimensions = new ArrayNode { Expr = AstFactory.BuildIntNode(0) }
                    }),
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(1),
                    new IdentifierNode(packedId)
                    {
                        ArrayDimensions = new ArrayNode { Expr = AstFactory.BuildIntNode(1) }
                    })
            };
        }
    }

    [NodeName(/*NXLT*/"ReplaceByCondition")]
    [NodeCategory(BuiltinNodeCategories.CORE_LISTS_ACTION)]
    [NodeDescription(/*NXLT*/"ReplaceByConditionDescription", typeof(Resources))]
    [IsDesignScriptCompatible]
    public class Replace : NodeModel
    {
        public Replace(WorkspaceModel workspace) : base(workspace)
        {
            InPortData.Add(new PortData(/*NXLT*/"item", Resources.ReplacePortDataItemToolTip));
            InPortData.Add(new PortData(/*NXLT*/"replaceWith", Resources.ReplacePortDataReplaceWithToolTip));
            InPortData.Add(new PortData(/*NXLT*/"condition", Resources.ReplacePortDataConditionToolTip));

            OutPortData.Add(new PortData(/*NXLT*/"var", Resources.ReplacePortDataResultToolTip));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(
            List<AssociativeNode> inputAstNodes)
        {
            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    AstFactory.BuildFunctionCall("__Replace", inputAstNodes))
            };
        }
    }
}
