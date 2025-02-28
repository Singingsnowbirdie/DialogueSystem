using DialogueSystem.DialogueEditor;
using Gameplay.UI.ReactiveViews;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace UI
{
    public class DialogueUIModel : UIModel
    {
        public ReactiveProperty<string> SpeakerName = new ReactiveProperty<string>();
        public ReactiveProperty<string> DialogueText = new ReactiveProperty<string>();
    }

    public class DialogueView : UIView
    {
        [SerializeField] private TextMeshProReactiveStringView _speakerNameTF;
        [SerializeField] private TextMeshProReactiveStringView _dialogueTF;

        public override void OnSetModel(UIModel uiModel)
        {
            DialogueUIModel model = uiModel as DialogueUIModel;

            _speakerNameTF.SetUIModel(model.SpeakerName);
            _dialogueTF.SetUIModel(model.DialogueText);
        }



        //public Text speakerNameText;
        public Text dialogueText;
        public Button nextButton;

        public void UpdateDialogue(string speakerName, string dialogue)
        {
            speakerNameText.text = speakerName;
            dialogueText.text = dialogue;
        }

        public void SetNextButtonCallback(System.Action callback)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => callback());
        }
    }




    public class DialoguePresenter : IStartable
    {
        private readonly DialogueUIModel _model;
        private readonly DialogueView _view;

        public DialoguePresenter(DialogueUIModel model, DialogueView view)
        {
            _model = model;
            _view = view;
        }

        public void Start()
        {
            // Подписка на изменения текста
            _model.SpeakerName.Subscribe(name => _view.UpdateDialogue(name, _model.DialogueText.Value));
            _model.DialogueText.Subscribe(text => _view.UpdateDialogue(_model.SpeakerName.Value, text));

            // Подписка на кнопку "Далее"
            _view.SetNextButtonCallback(_model.MoveToNextNode);

            // Запуск диалога (для теста)
            DialogueGraph graph = Resources.Load<DialogueGraph>("DialogueGraph");
            _model.StartDialogue(graph);
        }

        //public ReactiveProperty<DialogueNode> CurrentNode = new ReactiveProperty<DialogueNode>();

        //public void StartDialogue(string speakerName, DialogueGraph graph)
        //{
        //    SpeakerName.Value = speakerName;



        //    CurrentNode.Value = graph.startNode;
        //    UpdateDialogue();
        //}

        //public void MoveToNextNode()
        //{
        //    if (CurrentNode.Value != null)
        //    {
        //        CurrentNode.Value = CurrentNode.Value.GetNextNode();
        //        UpdateDialogue();
        //    }
        //}

        //private void UpdateDialogue()
        //{
        //    if (CurrentNode.Value != null)
        //    {
        //        SpeakerName.Value = CurrentNode.Value.speakerName;
        //        DialogueText.Value = CurrentNode.Value.dialogueText;
        //    }
        //    else
        //    {
        //        Debug.Log("Диалог завершен.");
        //    }
        //}
    }
}


