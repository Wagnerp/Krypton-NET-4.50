﻿// *****************************************************************************
// BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
//  © Component Factory Pty Ltd, 2006-2018, All rights reserved.
// The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to licence terms.
// 
//  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV) 2017 - 2018. All rights reserved. (https://github.com/Wagnerp/Krypton-NET-4.5)
//  Version 4.5.0.0  www.ComponentFactory.com
// *****************************************************************************

using System.ComponentModel;
using System.Diagnostics;

namespace ComponentFactory.Krypton.Toolkit
{
	/// <summary>
	/// Implement storage for just the combo part of a combo box state.
	/// </summary>
	public class PaletteComboBoxStates : Storage
	{
		#region Instance Fields

	    #endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the PaletteComboBoxStates class.
		/// </summary>
        /// <param name="inheritComboBox">Source for inheriting combo box values.</param>
        /// <param name="inheritItem">Source for inheriting item values.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public PaletteComboBoxStates(IPaletteTriple inheritComboBox,
                                     IPaletteTriple inheritItem,
                                     NeedPaintHandler needPaint)
		{
            Debug.Assert(inheritComboBox != null);
            Debug.Assert(inheritItem != null);

            // Store the provided paint notification delegate
            NeedPaint = needPaint;

            // Create storage that maps onto the inherit instances
            Item = new PaletteTriple(inheritItem, needPaint);
            ComboBox = new PaletteInputControlTripleStates(inheritComboBox, needPaint);
		}
		#endregion

		#region IsDefault
		/// <summary>
		/// Gets a value indicating if all values are default.
		/// </summary>
		[Browsable(false)]
		public override bool IsDefault => (ComboBox.IsDefault &&
		                                   Item.IsDefault);

	    #endregion

        #region SetInherit
        /// <summary>
        /// Sets the inheritence parent.
        /// </summary>
        /// <param name="inheritComboBox">Source for inheriting combo box values.</param>
        /// <param name="inheritItem">Source for inheriting item values.</param>
        public void SetInherit(IPaletteTriple inheritComboBox,
                               IPaletteTriple inheritItem)
        {
            ComboBox.SetInherit(inheritComboBox);
            Item.SetInherit(inheritItem);
        }
        #endregion

        #region PopulateFromBase
        /// <summary>
        /// Populate values from the base palette.
        /// </summary>
        /// <param name="state">Palette state to use when populating.</param>
        public void PopulateFromBase(PaletteState state)
        {
            ComboBox.PopulateFromBase(state);
            Item.PopulateFromBase(state);
        }
        #endregion

        #region ComboBox
        /// <summary>
        /// Gets access to the combo box appearance.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining combo box appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteInputControlTripleStates ComboBox { get; }

	    private bool ShouldSerializeComboBox()
        {
            return !ComboBox.IsDefault;
        }
        #endregion

        #region Item
        /// <summary>
        /// Gets access to the item appearance.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining item appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple Item { get; }

	    private bool ShouldSerializeItem()
        {
            return !Item.IsDefault;
        }
        #endregion

		#region Implementation
		/// <summary>
		/// Handle a change event from palette source.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="needLayout">True if a layout is also needed.</param>
        protected void OnNeedPaint(object sender, bool needLayout)
		{
			// Pass request from child to our own handler
			PerformNeedPaint(needLayout);
		}
		#endregion
	}
}
