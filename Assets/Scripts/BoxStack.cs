using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BoxStack : Stack<Box> {
	public new void Push (Box b) {
		b.spriteRenderer.sortingOrder = this.Count + 1;
		
		if(this.Count > 0) {
			var currentTop = base.Pop();
			currentTop.Interactable = false;

			base.Push(currentTop);
		}
		base.Push(b);

		// remove entry from boxstacks dictionary if exists
		var boxstacks = GameManager.Instance.BoxStacks;
		if (boxstacks.ContainsKey(b.BoxNum) && boxstacks[b.BoxNum].Count < 1) {
			boxstacks.Remove(b.BoxNum);
		}
	}

	public new Box Pop () {
		var popped = base.Pop();

		if (this.Count > 0) {
			var currentTop = base.Pop();
			currentTop.Interactable = true;
			base.Push(currentTop);
		}

		return popped;
	}
}

