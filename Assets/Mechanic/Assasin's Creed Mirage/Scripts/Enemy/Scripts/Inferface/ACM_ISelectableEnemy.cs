using System.Collections.Generic;

public interface ACM_ISelectableEnemy
{
   void OnSelectable(ChainAssassination context);
   void OnSelected(ChainAssassination context);
   void OnDeselected(ChainAssassination context);
   void OnUnSelect(ChainAssassination context);
}
