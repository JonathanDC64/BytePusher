# BytePusher

This is an implementation of the BytePusher VM using pure C# for the backend and MonoGame for the front end.

[BytePusher Specification](https://esolangs.org/wiki/BytePusher)

# Controls

The keys on the left represent keys on your physical keyboard.

The keys on the right represent keys on the virtual machine.

Each key on the left is respectively mapped to the key of the right.

Ex: The Q key on your keyboard is mapped to the 4 key on the BytePusher VM.

<table>
<tr><th>Keyboard</th><th>=></th><th>VM Keyboard</th></tr>
<tr><td>

|  |  |  |  |   
|---|---|---|---|
| 1 | 2 | 3 | 4 |    
| Q | W | E | R |   
| A | S | D | F |    
| Z | X | C | V | 

</td><td>

||
|---
|=>|
|=>|
|=>|
|=>|

</td><td>

|  |  |  |  |   
|---|---|---|---|
| 1 | 2 | 3 | C |    
| 4 | 5 | 6 | D |   
| 7 | 8 | 9 | E |    
| A | 0 | B | F | 


</td></tr> </table>

 



