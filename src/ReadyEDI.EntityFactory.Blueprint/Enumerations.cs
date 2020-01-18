using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Enumerations : Collection<Enumeration>
	{
		public Enumerations()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Enumeration>(this);
		}
	}
}
