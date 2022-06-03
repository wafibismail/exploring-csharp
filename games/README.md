## Introduction to Game Design, Prototyping, and Development
For this section, I am following the lessons in a book with this exact title, by ***Jeremy Gibson Bond*** <br>
<br>

Note on notations within codes in book:
- old lines in normal weight
- **new lines in bold weight**
- ... - ellipses indicate skipped lines
- {...} - ellipses in braces indicate skipped code from a pre-existing function/method

DEBLOCKING (e.g. in Bartok):
- break up blocks of (similar) cards.
- if not done, the next game will end much quicker as players are very likely dealt cards that match each other

How to DEBLOCK?
- shuffle 7x
- doesn't work? use one of these:
  - deal the cards into 7 piles, then shuffle them together
  - deal the cards out face-down into a large spread out pool, mix them like mixing water (domino style)
  - play 52 pickup: throw all the cards on the floor and pick them up

After PLAYTESTs, ask:
- Is the difficulty level appropriate for the intended audience? too difficult / too easy / just right?
- Is the outcome of the game based on strategy or chance? randomness play a strong role / game too deterministic? -such that after one player has taken the lead, others cannot catch up
- Does the game have meaningful, interesting decisions? On your turn, do you have several choices, & are they interesting?
- Is the game interesting when it's not your turn? Do you have any effect on the other players' turn? Do their turn turns have any immediate effect on you?

E.g. in BARTOK, the paper version:
- ask players to write down their answers to these questions individually, then discuss as a group. This keeps the responses diverse, uninfluenced.

Game design is merely a process:
- Incrementally MODIFY THE RULES
  - changing very few things between each playtest
- Playtest the game with the new rules
- Analyze how the feel of the game is altered by the new rules
- Design new rules that may move the feel of the game in the desired direction
- Repeat until satisfied

I.e., ITERATIVE DESIGN <br>
<br>

WATCH OUT FOR PLAYTESTING FLUKES
- The occurence of an unlikely event / external factor e.g. a weird shuffle may cause a single play to feel really different from the others. Be sure to play through the game multiple times so as to avoid making rule changes based on a fluke.

GAME FEEL
- do you want it to be
  - exciting?
  - cutthroat?
  - leisurely and slow?
  - based more on strategy, or chance?
- once there is a general idea of the desired feel, come up with additional rules that can push the feel of the game in that direction

Tips regarding designing new rules:
- Change only one thing in between each playtest
  - so then it is easier to pinpoint which changes influence a certain effect towards a feel
- The bigger the change, the more frequent the tests need to be to ensure the subsequent change decision is not based on flukes
  - better to keep the changes subtle such that one or two plays can say alot of how each change affects the feel
- Change a number (no matter how small) and you change the experience

Additionally:
- adding new rules can be easier when the playtests are done with friends
  - in person rather than with digital prototypes
    - This highlights the importance of paper prototypes, even when designing digital ones

GAME DEFINITION 1: ACCORDING TO BERNARD SUITE:
- Short: A game is a voluntary attempt to overcome unnecessary obstacles
- Lengthier: To play a game is to attempt to achieve a specific state of affairs, using only means permitted by rules, where the rules prohibit use of more efficient in favor of less efficient means, and where the rules are accepted just because they make possible such activity.

GAME DEFINITION 2: ACCORDING TO SID MEIER:
- Short: A game is a series of interesting decisions
- Further: An interesting decision is generally one where:
  - The player has multiple valid options from which to choose
  - Each option has both positive and negative potential consequences
  - The outcome of each option is predictable but not guaranteed

GAME DEFINITION 3: ACCORDING TO TRACY FULLERTON
- Short: A game is a closed, formal system that engages players in a structured conflict and resolves its uncertainty in an unequal outcome
- Further: This definition is also a list of elements that designers can modify in their games:
  - Formal elements - which differentiate a game from other types of media
    - i.e. rules, procedures, players, resources, objectives, boundaries, conflict & outcome.
  - (Dynamic) systems - Methods of interactions that evolve as the game is played
  - Conflict structure - The ways in which players interact with each other
  - Uncertainty - The interaction between randomness, determinism, and player strategy
  - Unequal outcome - How does the game end? Do players win, lose, or something else?

GAME DEFINITION 4: ACCORDING TO JESSE SCHELL
- Short: A game is a problem-solving activity, approached with a playful attitude.
- Further:
  - It is the playful attitude of the player that makes something a game.
  - It is because of their playful attitude that players will happily follow the rules of a game even though there might be an easier way to achieve the stated goal of the game (e.g. hacking? not stated in the book, it just popped up in my head)
- Therefore another design goal is to encourage a playful attitude. Design the game to encourage players to enjoy the limitations placed on them by the rules, i.e. balance the game well.

GAME DEFINITION 5: ACCORDING TO KEITH BURGUN
- Short: A game is a system of rules in which agents compete by making ambiguous, endogenously meaningful decisions
- Core: Is that - the player is making choices and that those choices are both ambiguous (the player doesn't know exactly what the outcome of the choice will be) and endogenously meaningful (the choice is meaningful because it has a noticeable effect upon the game system)

Notes not taken, but worth going back to as it makes clear why I'm writing the next question:
- Why Care About the Definition of Game? (Page 56)

Reasons for playing games:
- Humans desire conflict
  - The rules that limit behaviour are there precisely because the challenge of those limitations is appealing to players
- Humans desire the experience of being someone else
- Humans desire excitement
  - In addition, what separates games from other forms of media is that the player is actively taking part in the excitement rather than vicariously absorbing it

The point brought up in the book as to why considering these reasons is significant is that doing so provides a basis for understanding both
- their importance in society and
- the positive and negative effects that games can have on the people who play them.

As a game designer, understanding these needs and respecting their power (e.g. them being potentially addictive, on the negative side) can be incredibly useful.

The word "game" is used differently depending who uses it e.g. who the player is, and the word is constantly evolving

How is understand definitions critical?
- Definitions help us understand what people expect from our games
  - especially when working on a specific genre or towards a specific audience
    - understanding how that audience defines the term can help us in crafting better games for them 
- Definitions can lead to understanding not only
  - the core of the defined concept, but also
  - the periphery (which games fit the definition and which do not)
- Definitions can help in speaking eloquently with others in the field

LUDOLOGY - The study of games and game designs <br>
<br>

Common frameworks for LUDOLOGY:
- MDA - Mechanics, Designs and Aesthetics
  - This framework provides important points to consider about the difference between how designers and players approach games.
- FDD - Formal, Dramatic and Dynamic Elements
  - This framework framework focuses on concrete analytical tools to help designers make better games and push their ideas further
- Elemental Tetrad - splits games into four core elements:
  - mechanics
  - aesthetics
  - story
  - techology

In the context of MDA (as these same words may be defined differently in other frameworks):
- Mechanics: The particular components of the game at the level of data representation and algorithms
- Dynamics: The runtime behaviour of the mechanics acting on player inputs and each other's outputs over time
- Aesthetics: The desirable emotional response evoked in the player when she interacts with the game system

Designer and Player Views of a Game
- Designer:
  - first, consider games in terms of the aesthetics, the emotions that the designer wants players to feel while playing the game
  - then work backward to the kind of dynamic play that would prompt those feelings
  - then finally to the gameplay mechanics that will create those dynamics
- Player:
  - first experience the mechanics (e.g. reading the written rules for the game)
  - then experiencing the dynamics by playing the game
  - then finally experiencing the aesthetics that were initially envisioned by the designer

```
            Designers
    <-- <-- <-- <-- <-- <--o
MECHANICS - DYNAMICS - AESTHETICS
    o--> --> --> --> --> -->
             Players
```

E.g. children's games
- often designed to make each player feel like they are doing well and have a chance to win up until the very end.
  - players must feel that the end of the game is not inevitable and must be able to hope for good luck throughout the game

E.g. in the original Snake and Ladders (or Moksha Patamu), the desired aesthetic is for the players to experience hope, reversal of fortune, and excitement in a game in which the players never make any choice
- the *mechanic* is the inclusion of the snakes and ladders
- the *dynamic* is the intersection of the two where the act of the players encountering the mechanics leads to:
- the *aesthetic* feelings of hope and excitement

Where MDA seeks to help both designers and critics better understand and discuss games, FDD help students in more effectively design games

In the context of the FDD framework:
- Formal elements - provide structure of a game; include things like
  - rules
  - resources
  - boundaries
- Dramatic elements - the story and narrative of the game, including the premise. These
  - tie the game together,
  - help players understand the rules, and
  - encourage the player to become emotionally invested in the game
- Dynamic elements - the game in motion; After players turn the rules into actual gameplay, the game has moved into dynamic elements. These include things like
  - strategy
  - behaviour
  - relationships between game entities

FORMAL ELEMENTS
- Seven are proposed that differentiate games from other forms of media:
  - Player interaction pattern: how the players interact
    - single-player
    - one-on-one
    - team-vs-team
    - multilateral (multiple vs each other)
    - cooperative
    - multiple players each against the same system
  - Objectives
    - what are the players trying to achieve?
    - when has someone won?
  - Rules: These limit the players' actions by telling them what they may or may not do in the game. These can be
    - explicitly written
    - implicitly understood 
  - Procedures:
    - the types of actions taken by the players in the game
  - Resources: elements that have value in the game
    - money
    - health
    - items
    - properties
  - Boundaries:
    - where does the game end and reality begin?
  - Outcome: how did the game end? There are
    - final outcome e.g. who wins in a game of chess
    - incremental outcome e.g. when a player defeats an enemy and levels up
- Can be see in a way such that when one of these is removed, the game ceases to be a game

DRAMATIC ELEMENTS
- help make the rules and resources more understandable to players and can give players greater emotional investment in the game
- three types are presented:
  - Premise: the basic story of the game world
    - in Monopoly: each of the players is a real-estate developer trying to get a monopoly on corporate real estate in Atlantic City, New Jersey
    - in Donkey Kong: the player is trying to single-handedly save Pauline from a gorilla that has kidnapped her.
  - Character: individuals around whom the story revolves. Designers must choose whether the protagonist will act as:
    - an avatar for the player, conveying the emotions, desires and intentions of the player into the game world
    - as a role that the player must take on, so that the player acts out the wishes of the game character
      - this latter one is more common and easier to implement
  - Story: the plot of the game, which encompasses the actual narrative that takes place through the course of the game
    - the **Premise** sets the stage on which the story takes place.

DYNAMIC ELEMENTS
- are the elements that occur only when the game is being played. The core CONCEPTS of these dynamic elements are:
  - Emergence:
    - Simple rules lead to complex and unpredictable behaviour
      - It is one of the designer's most important jobs to attempt to understand the emergent implications of the rules in the game
  - Emergent narrative:
    - FDD model recognizes that narrative can also be dynamic (as opposed to only the behaviour of mechanics)
    - numerous narratives can emerge from the gameplay itself, where e.g. players are put in extra-normal situations leading to interesting stories
  - Playtesting is the only way to understand dynamics
    - Experienced game designers can often make better predictions about dynamic behaviour and emergence than novice designers
      - though no one understands exactly how the dynamics of a game will play out without playtesting them.

THE ELEMENTAL TETRAD - four basic elements of games:
- Mechanics - the rules for interaction between the player and the game,
  - containing things like:
    - rules
    - objectives
    - other formal elements
  - different from mechanics presented by MDA in the sense that in this there is differentiation between game mechanics and the underlying technology that enables them
- Aesthetics - describes how the game is percieved by vision, sound, smell, taste and touch.
  - covers everything from the soundtrack of the game to the character models, packaging and cover art.
- Technology - covers all the underlying technology that makes the game work
  - most obviously refers to things such as console hardware, computer software, rendering pipelines, etc.
  - also covers technological elements in board games e.g.
    - type and number of dice that are chosen
    - whether dice or a a deck of cards are used as the randomizer
    - various stats and tables used to determine the outcomes of actions.
- Story - covers all that is covered by ***Dramatic Elements*** in FDD including both premise and character

Four elements interrelating with each other:

```
        Aesthetics
       /     |    \
Mechanics----+----Story
       \     |    /
        Technology
```

The Elemental Tetrad touches on static elements rather than dynamic play of the game

SUMMARY - PART 1 CHAPTER 2
- MDA demonstrates: Players and designers approach games by different directions; Designers can be more effective by learning to see their games from the players' perspective
- FDD breaks game design into specific components which can individually considered and improved
- ELEMENTAL TRIAD separates the basic elements of a game into the sections that are generally assigned to various teams e.g.:
  - game designers handle mechanics
  - artists handle aesthetics
  - writers handle story
  - programmers handle technology

The Layered Tetrad combines and expands on the ideas presented in all the above frameworks <br>
_______________________________________<br>
<br>

COMPONENT ORIENTED DESIGN:
- Component Pattern:
  - Core idea: to group closely related functions and data into a single class while at the same time keeping each class as small and focused as possible.
  - Is what each GameObject in Unity is based on, e.g., the components:
    - *Transform* handles position, rotation, scale and hierarchy.
    - *Rigidbody* handles motion and physics
    - *Colliders* handle actual collision and the shape of the collision volume
  - Gives us smaller, shorter classes to work with, meaning easier to
    - code
    - share with others
    - reuse
    - debug
  - However also requires a decent amount of forethought, thus may not be suitable in all situations.
- Example in Boid folder

# PAUSED ON PAGE 546 - Attractor script

#### Other Books

The [author](#introduction-to-game-design-prototyping-and-development) also noted several other books recommended books (refer to PREFACE for the explanation):
- Game Design Workshop: A Playcentric Approach to Creating Innovative Games
  - by Tracy Fullerton, Christopher Swain, and Steven Hoffman
- The Art of Game Design: A Book of Lenses
  - by Jesse Schell
- The Grasshopper: Games, Life and Utopia
  - by Bernard Suits
- Level Up!: The Guide to Great Video Game Design
  - by Scott Rogers
- Imaginary Games
  - by Chris Bateman
- Game Programming Patterns
  - by Robert Nystrom
- Game Design Theory
  - by Keith Burgun
