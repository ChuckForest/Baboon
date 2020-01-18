using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class InterfaceCollections : Collection<Interface>
	{
		public InterfaceCollections()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Interface>(this);
		}
	}
}
