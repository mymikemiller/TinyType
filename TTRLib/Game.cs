using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTRLib
{
    public class Game
    {
        private List<Level> Levels { get; set; }
        public int CurrentLevelNum { get; private set; }

        public Level CurrentLevel { get { return Levels[CurrentLevelNum]; } }

        public Game()
        {
            Levels = LoadLevels();
            CurrentLevelNum = 6;
        }

        private List<Level> LoadLevels()
        {
            List<Level> levels = new List<Level>();
            //string line;
            int counter = 0;

            /*
            System.IO.StreamReader file = new System.IO.StreamReader("WordList.txt");
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
                counter++;
            }

            file.Close();
             * */

            string lettersSeen = "";


            foreach (string line in GetString().Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.TrimEnd().Split(' ');
                char character = parts[0][0];
                List<string> words = new List<string>();
                words.AddRange(parts);
                words.RemoveAt(0); // Remove the line's character

                bool firstAppearance;
                if (firstAppearance = !lettersSeen.Contains(character))
                {
                    lettersSeen += character;
                }

                levels.Add(new Level(counter, character, firstAppearance, words));
                counter++;
            }


            return levels;
        }
        
        private string GetString()
        {
            return @"e 
a a
t at eat tea
s as see set sea
e see set eat sea tea
a a as sea at eat tea
t at eat tea set
s as see set sea
l all let
r are art ear
l all less last let else salt tell late sell sale tale tall
e see less else sell sale sea ease
r are real area rare ear
a a are all real area rare ear
i i air lie ill is rise sail sir
t it at art let rate tell late eat tree tie tea till tale tall tear
s is as its see set test site rest rise star sea sit seat east ease sir
i is it i its site sit tie
d add idea side ad die dead sad dad
n in an and need end send sand
l all less else sell sale lie ill sail
d add idea deal lead ad die ideal dead dad
r are air read real area rare ride red rid dare dear ear
n and in an need end rain near earn inner
e need end idea die dead
a and a an add idea ad dead dad
i in i idea die air rain ride rid inner
t it at eat net tie tea neat
o on one no none to not into too note tone toe
c can once ice nice act coat cat
s is as so see sense soon season sea nose ease noise son
o so on one no soon season nose noise none son
d do add idea ad die dead dad odd
c decide ice code access case
l all local call oil cell cool lie ill
r are or air care area car career rare rice race error ear
n in on can an one no once nice none
p piece open peace pipe pain cap pin pace panic pie piano pen pop
u union up upon cup our run occur pure upper ruin pour
p up piece open upon peace pipe pain cap cup pin pace panic pie piano pen pop
u up upon union cup our run occur pure upper ruin pour
e pipe pie one open none pen
o pop on one no open upon opinion union none piano
a a an pain piano can peace cap pace panic
i i pipe pie in opinion union pain pin piano
d do add idea due deep ad die dead dad odd
t to it at out too put top pot eat tap tie tip tea potato poet toe
c piece ice peace cap cup pace
s is as so use see us access case cause issue success sea ease accuse
m come me mom some same music mouse seem assume mess miss
n in on can an one no once nice union none announce
l all local line call oil alone cell clean cancel cool annual lie loan nail clue ill uncle
m come me common name main mean man income minimum mom menu mine
r are or more our air area run room rain rare remain near earn minor nor manner arm iron error inner mirror ruin ear
o on one no union mom none
p up map pipe pie poem pop
u up upon minimum union menu
h he hi home him hope
g go game image age ago egg
h he hi home him high huge
g go high age ago egg huge
i i hi high image him
d do good had guide add idea head dog due ahead edge ad die dead dig god hide dad odd
c each choice ice coach decide code
m game me home image him mom
y you may my eye guy
p up map page pipe gap pie poem pop
e game me image age egg
y you may my eye guy
a a may game image age ago
h he hi home him yeah
u you huge guy up much cup
n in on an one no any anyone union none honey
t the to that it at they out too heat hot yet hit eat tie hate tooth tea hat toe youth
o you to out too hot tooth toe youth
g go high age ago egg huge guy
s is as so use see us say easy issue yes gas sea essay guess ease
l all oil goal legal log lie illegal lay league ill ugly leg
f of if off fee life feel full fall fly fail file fully fill golf fuel
r are or your our air year area rare agree gear garage argue error ear
c ice care car occur career cry carry rice race courage grocery
m may my game me image mom
f of if off fee face office coffee
p up map pipe pie poem pop
b be big bag bug by buy maybe baby boy
h he hi home him hope
y you eye yeah may my
b be by buy baby boy
i i if hi him pipe pie
d do day body add idea bad due ad die dead bed buddy dad bid odd
u you buy due buddy medium mud
v above video avoid divide have heavy behave
g go big age ago egg bag bug guy
f of if off fee food feed
v above give video avoid divide
w we few view wave owe wife
k ok week weak wake book bake bike
w we few view wave owe wife
n in on an one even no new own now oven union win none wine
k week weak ok wake know known knee
b be book web bake bike
o owe ok book above on one no know own now known oven union bone none
m make me mom move movie
y you way away key eye
h how who he hi hook
e we week owe weak wake
v view wave have heavy move movie
a a wave weak wake have
c cook ice voice kick cake cookie
f of if off fee face office coffee
p up pipe pie pop keep peak
t to it at out too fat fit foot eat tie tea toe
g go give age ago egg
b be above big bag bug
w we web view wave owe
s is as so was use see us boss basis bus issue base abuse sea wise ease
l will all well able law oil below allow low ball wall bowl blue lie bill blow lab bell ill
k book week bake bike weak ok wake
r are or work our were air area war rare aware raw wear error row worker ear
u our rub rule blue use us sure user bus issue serious abuse assure usual
y you way away key eye
x box relax fix tax exist extra exit text
q require equally equal square qualify
x box relax fix tax exist extra exit text
v view wave very over every ever review vary river arrive
q require equally equal square qualify
i i view air require review river arrive
d do add idea due ad die dead dad odd
m me exam mix maximum mom
j joke major jury job judge
h he hi home him had head ahead hide
f of if off fix fee
j joke major jury job judge
w we owe few wife how who
b be job web box bad bed bid
k week weak joke ok wake
n in on an one no join union none knee
x box fix exam mix maximum examine
o joke ok on one no join union none
g go age ago egg again gain engage engine king gene
q unique queen require equally equal qualify
z zone magazine amazing organized organize";
        }
    }

}
