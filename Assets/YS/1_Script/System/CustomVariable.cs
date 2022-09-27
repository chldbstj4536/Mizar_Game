using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public abstract class CustomVariable
    {
        public abstract void Set(bool value, ARITHMETIC_CALC calcType);
        public abstract void Set(int value, ARITHMETIC_CALC calcType);
        public abstract void Set(float value, ARITHMETIC_CALC calcType);
        public abstract bool Compare(bool value, COMPARE_TYPE compareType);
        public abstract bool Compare(int value, COMPARE_TYPE compareType);
        public abstract bool Compare(float value, COMPARE_TYPE compareType);
        public abstract CustomVariable Instantiate();
    }
    [System.Serializable]
    public class BoolVariable : CustomVariable
    {
        [HideLabel, SerializeField]
        private bool data;
        public BoolVariable(bool value) { data = value; }
        public override void Set(bool value, ARITHMETIC_CALC calcType)
        {
            switch (calcType)
            {
                case ARITHMETIC_CALC.SUBSTITUTE:
                    data = value;
                    break;
                case ARITHMETIC_CALC.ADD:
                case ARITHMETIC_CALC.SUBTRACT:
                case ARITHMETIC_CALC.MULTIPLY:
                case ARITHMETIC_CALC.DIVIDE:
                    throw new UnityException("올바르지 않은 연산");
            }
        }
        public override void Set(int value, ARITHMETIC_CALC calcType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override void Set(float value, ARITHMETIC_CALC calcType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override bool Compare(bool value, COMPARE_TYPE compareType)
        {
            switch (compareType)
            {
                case COMPARE_TYPE.EQUAL:
                    return data == value;
                case COMPARE_TYPE.NOT_EQUAL:
                    return data != value;
                case COMPARE_TYPE.GREATER:
                case COMPARE_TYPE.EQUAL_GREATER:
                case COMPARE_TYPE.LESS:
                case COMPARE_TYPE.EQUAL_LESS:
                default:
                    throw new UnityException("올바르지 않은 연산입니다");
            }
        }
        public override bool Compare(int value, COMPARE_TYPE compareType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override bool Compare(float value, COMPARE_TYPE compareType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override CustomVariable Instantiate() { return new BoolVariable(data); }
    }
    [System.Serializable]
    public class IntVariable : CustomVariable
    {
        [HideLabel, SerializeField]
        private int data;
        public IntVariable(int value) { data = value; }
        public override void Set(bool value, ARITHMETIC_CALC calcType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override void Set(int value, ARITHMETIC_CALC calcType)
        {
            switch (calcType)
            {
                case ARITHMETIC_CALC.SUBSTITUTE:
                    data = value;
                    break;
                case ARITHMETIC_CALC.ADD:
                    data += value;
                    break;
                case ARITHMETIC_CALC.SUBTRACT:
                    data -= value;
                    break;
                case ARITHMETIC_CALC.MULTIPLY:
                    data *= value;
                    break;
                case ARITHMETIC_CALC.DIVIDE:
                    data /= value;
                    break;
            }
        }
        public override void Set(float value, ARITHMETIC_CALC calcType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override bool Compare(bool value, COMPARE_TYPE compareType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override bool Compare(int value, COMPARE_TYPE compareType)
        {
            switch (compareType)
            {
                case COMPARE_TYPE.EQUAL:
                    return data == value;
                case COMPARE_TYPE.NOT_EQUAL:
                    return data != value;
                case COMPARE_TYPE.GREATER:
                    return data < value;
                case COMPARE_TYPE.EQUAL_GREATER:
                    return data <= value;
                case COMPARE_TYPE.LESS:
                    return data > value;
                case COMPARE_TYPE.EQUAL_LESS:
                    return data >= value;
                default:
                    throw new UnityException("올바르지 않은 연산입니다");
            }
        }
        public override bool Compare(float value, COMPARE_TYPE compareType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override CustomVariable Instantiate() { return new IntVariable(data); }
    }
    [System.Serializable]
    public class FloatVariable : CustomVariable
    {
        [HideLabel, SerializeField]
        private float data;
        public FloatVariable(float value) { data = value; }
        public override void Set(bool value, ARITHMETIC_CALC calcType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override void Set(int value, ARITHMETIC_CALC calcType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override void Set(float value, ARITHMETIC_CALC calcType)
        {
            switch (calcType)
            {
                case ARITHMETIC_CALC.SUBSTITUTE:
                    data = value;
                    break;
                case ARITHMETIC_CALC.ADD:
                    data += value;
                    break;
                case ARITHMETIC_CALC.SUBTRACT:
                    data -= value;
                    break;
                case ARITHMETIC_CALC.MULTIPLY:
                    data *= value;
                    break;
                case ARITHMETIC_CALC.DIVIDE:
                    data /= value;
                    break;
            }
        }
        public override bool Compare(bool value, COMPARE_TYPE compareType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override bool Compare(int value, COMPARE_TYPE compareType) { throw new System.InvalidCastException("유효하지 않은 타입"); }
        public override bool Compare(float value, COMPARE_TYPE compareType)
        {
            switch (compareType)
            {
                case COMPARE_TYPE.EQUAL:
                    return data == value;
                case COMPARE_TYPE.NOT_EQUAL:
                    return data != value;
                case COMPARE_TYPE.GREATER:
                    return data < value;
                case COMPARE_TYPE.EQUAL_GREATER:
                    return data <= value;
                case COMPARE_TYPE.LESS:
                    return data > value;
                case COMPARE_TYPE.EQUAL_LESS:
                    return data >= value;
                default:
                    throw new UnityException("올바르지 않은 연산입니다");
            }
        }
        public override CustomVariable Instantiate() { return new FloatVariable(data); }
    }
    public enum LOGIC_CALC
    {
        AND,
        OR
    }
    public enum ARITHMETIC_CALC
    {
        [LabelText("=")]
        SUBSTITUTE,
        [LabelText("+")]
        ADD,
        [LabelText("-")]
        SUBTRACT,
        [LabelText("×")]
        MULTIPLY,
        [LabelText("÷")]
        DIVIDE,
    }

    public enum COMPARE_TYPE
    {
        [LabelText("==")]
        EQUAL,
        [LabelText("!=")]
        NOT_EQUAL,
        [LabelText("<")]
        GREATER,
        [LabelText("<=")]
        EQUAL_GREATER,
        [LabelText(">")]
        LESS,
        [LabelText(">=")]
        EQUAL_LESS
    }
    public enum ADDABLE_TYPE
    {
        [LabelText("상태")]
        BOOL,
        [LabelText("정수")]
        INT,
        [LabelText("실수")]
        FLOAT
    }
    [System.Serializable]
    public struct ChangeVariableInTable
    {
        [ValidateInput("Validate", "상태 변수는 =만 사용 가능합니다.", ContinuousValidationCheck = true)]
        [HorizontalGroup("변수 변경 그룹", Width = 0.1f)]
        [HideLabel, SerializeField]
        private ADDABLE_TYPE varType;
        [HorizontalGroup("변수 변경 그룹")]
        [HideLabel, SerializeField]
        private string varName;
        [HorizontalGroup("변수 변경 그룹", Width = 0.1f)]
        [HideLabel, SerializeField]
        private ARITHMETIC_CALC calcType;
        [HorizontalGroup("변수 변경 그룹", Width = 0.25f)]
        [HideLabel, SerializeField, ShowIf("varType", ADDABLE_TYPE.BOOL)]
        private bool valueBool;
        [HorizontalGroup("변수 변경 그룹", Width = 0.25f)]
        [HideLabel, SerializeField, ShowIf("varType", ADDABLE_TYPE.INT)]
        private int valueInt;
        [HorizontalGroup("변수 변경 그룹", Width = 0.25f)]
        [HideLabel, SerializeField, ShowIf("varType", ADDABLE_TYPE.FLOAT)]
        private float valueFloat;

        public void Calculate()
        {
            var table = GameManager.Instance.VariablesTable;

            if (!table.ContainsKey(varName))
                    throw new UnityException("존재하지 않는 변수명입니다.");
            Debug.Log($"{varName}를 {calcType} 연산!");
            switch (varType)
            {
                case ADDABLE_TYPE.BOOL:
                    table[varName].Set(valueBool, calcType);
                    break;
                case ADDABLE_TYPE.INT:
                    table[varName].Set(valueInt, calcType);
                    break;
                case ADDABLE_TYPE.FLOAT:
                    table[varName].Set(valueFloat, calcType);
                    break;
            }
        }
        private bool Validate()
        {
            return !(varType == ADDABLE_TYPE.BOOL && calcType != ARITHMETIC_CALC.SUBSTITUTE);
        }
    }
    [System.Serializable]
    public struct CompareVariableInTable
    {
        [ValidateInput("Validate", "상태 변수는 ==, !=만 사용 가능합니다.", ContinuousValidationCheck = true)]
        [HorizontalGroup("변수 비교 그룹", Width = 0.1f)]
        [HideLabel, SerializeField]
        private ADDABLE_TYPE varType;
        [HorizontalGroup("변수 비교 그룹", Width = 0.4f)]
        [HideLabel, SerializeField]
        private string varName;
        [HorizontalGroup("변수 비교 그룹", Width = 0.1f)]
        [HideLabel, SerializeField]
        private COMPARE_TYPE compareType;
        [HorizontalGroup("변수 비교 그룹")]
        [HideLabel, SerializeField]
        [ShowIf("varType", ADDABLE_TYPE.BOOL)]
        private bool valueBool;
        [HorizontalGroup("변수 비교 그룹")]
        [HideLabel, SerializeField]
        [ShowIf("varType", ADDABLE_TYPE.INT)]
        private int valueInt;
        [HorizontalGroup("변수 비교 그룹")]
        [HideLabel, SerializeField]
        [ShowIf("varType", ADDABLE_TYPE.FLOAT)]
        private float valueFloat;
        [HorizontalGroup("변수 비교 그룹", Width = 0.12f)]
        [HideLabel, SerializeField]
        private LOGIC_CALC logicCalc;

        public LOGIC_CALC LogicCalc => logicCalc;

        public bool Compare()
        {
            var table = GameManager.Instance.VariablesTable;

            if (!table.ContainsKey(varName))
                throw new UnityException("존재하지 않는 변수명입니다.");

            switch (varType)
            {
                case ADDABLE_TYPE.BOOL:
                    return table[varName].Compare(valueBool, compareType);
                case ADDABLE_TYPE.INT:
                    return table[varName].Compare(valueInt, compareType);
                case ADDABLE_TYPE.FLOAT:
                    return table[varName].Compare(valueFloat, compareType);
                default:
                    throw new UnityException("올바르지 않은 연산입니다");
            }
        }
        public static bool Compare(CompareVariableInTable[] cmps)
        {
            bool result = false;

            foreach (var cmp in cmps)
            {
                result = cmp.Compare();

                if (result)
                {
                    if (cmp.LogicCalc == LOGIC_CALC.AND) continue;
                    else break;
                }
                else
                {
                    if (cmp.LogicCalc == LOGIC_CALC.AND) break;
                    else continue;
                }
            }

            return result;
        }

        private bool Validate()
        {
            return !(varType == ADDABLE_TYPE.BOOL && compareType != COMPARE_TYPE.EQUAL && compareType != COMPARE_TYPE.NOT_EQUAL);
        }
    }
}