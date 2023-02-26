using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;
using UnityEngine.Assertions;

namespace GuitarMan.MainMenuBehaviour
{
    [CreateAssetMenu(fileName = nameof(ButtonBehaviourDependencyStorage),
        menuName = nameof(MainMenuBehaviour) + "/" + nameof(ButtonBehaviourDependencyStorage))]
    public class ButtonBehaviourDependencyStorage : ScriptableObject
    {
        [SerializeField] private HandlerByButtonType[] _handlersByButtonTypes;

        public MonoScript GetHandler(ButtonType buttonType)
        {
            var targetData = _handlersByButtonTypes.FirstOrDefault(x => x.ButtonType == buttonType);

            Assert.IsNotNull(targetData,
                $"{nameof(ButtonBehaviourDependencyStorage)} {nameof(GetHandler)} " +
                $"— Can't find data with buttonType = {buttonType}");

            var behaviourHandler = targetData.BehaviourHandler;

            Assert.IsTrue(behaviourHandler.GetClass().GetTypeInfo().BaseType == typeof(AbstractButtonBehaviourHandler),
                $"{nameof(ButtonBehaviourDependencyStorage)} {nameof(GetHandler)} " +
                $"— Attached script must be inherited from {nameof(AbstractButtonBehaviourHandler)}");

            return targetData.BehaviourHandler;
        }
    }
}