using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Methods : Collection<Method>
	{
		public Methods()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Method>(this);
		}
	}
}
