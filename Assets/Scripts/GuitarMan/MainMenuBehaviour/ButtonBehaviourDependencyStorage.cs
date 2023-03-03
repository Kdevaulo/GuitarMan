using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.Assertions;

namespace GuitarMan.MainMenuBehaviour
{
    [AddComponentMenu(nameof(MainMenuBehaviour) + "/" + nameof(ButtonBehaviourDependencyStorage))]
    public class ButtonBehaviourDependencyStorage : MonoBehaviour
    {
        [SerializeField] private HandlerByButtonType[] _handlersByButtonTypes;

        public AbstractButtonBehaviourHandler GetHandler(ButtonType buttonType)
        {
            var targetData = _handlersByButtonTypes.FirstOrDefault(x => x.ButtonType == buttonType);

            Assert.IsNotNull(targetData,
                $"{nameof(ButtonBehaviourDependencyStorage)} {nameof(GetHandler)} " +
                $"— Can't find data with buttonType = {buttonType}");

            var behaviourHandler = targetData.BehaviourHandler;

            Assert.IsTrue(behaviourHandler.GetType().GetTypeInfo().BaseType == typeof(AbstractButtonBehaviourHandler),
                $"{nameof(ButtonBehaviourDependencyStorage)} {nameof(GetHandler)} " +
                $"— Attached script must be inherited from {nameof(AbstractButtonBehaviourHandler)}");

            return targetData.BehaviourHandler as AbstractButtonBehaviourHandler;
        }
    }
}