Item generation reference
=========================

Overview
--------

__Item__s are... well, items.

Items are generated using __Modifier__s. Modifiers give Items part of a silly name ("_x_ of Formidible Erections"), and have a list of Submodifiers.

__Submodifier__s are a two things: a list of item types, and a list of Modifications.

__Modification__s have an attached Attribute (technically, Attribute.Type) and, for now, a flag called "Guaranteed."


Item generation process
-----------------------

1.  A new, blank Item is created.

2.  The type (sword, shield, chest armor, etc.), rarity (common, uncommon, rare, etc.) and item level are chosen semi-randomly.

3.  The Modifiers that are going to be used on this item are selected, either one (a before-the-item-name adjective) or two (an adjective and an of-X).

4.  For each Modifier, check all of its Submodifiers. For each of those Submodifiers that apply to this Item (i.e. the Submodifier's "Type" is the same as this Item's Type), apply all of those Submodifiers' Modifications.

5.  Other attributes are randomly added to the Item based on its Rarity.