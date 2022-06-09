Bartok builds on the existing resources from Prospector Solitaire

Some adjustments are made to PrefabCard
- refer to page 857

SlotDef
- is defined in Layout as well as BartokLayout scripts
- the class definition for SlotDef is present in both
  - compiler error
  - author suggests to either:
    - delete the Layout script
      - I went with this, but due to the interscript references I also needed to delete
        - Prospector
        - ProspectorCard
    - comment out the SlotDef class definition

LINQ queries:
- work on arrays of values
- can be a bit slow,
  - may be best to restrict its usage where it's only called once e.g.
    - when a human player gets a card added to his/her hand

To do/check:
- more on LINQ queries