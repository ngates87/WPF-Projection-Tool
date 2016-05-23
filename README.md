# WPF-Projection-Tool
A C#/WPF based project tool created with theatre applications in mind. 


**About**

Created this project in the summer of 2014, wanted to do some projection mapping for a community theatre production of 'Wizard of Oz'. Initially looked at VPT7 would powerful, and had many features, however not create for a theatrical applications, as I wanted to create something that had more of a play list of images,videos, web-cam, etc. I also had issues with getting the remote network to work, and was just generally frustrating at times to use it like I wanted, so out of frustration, I just decided to write my own version, As I'm somewhat comfortable with with C#/WPF I decided to give that a shot. I found these following resources, https://www.youtube.com/watch?v=waeerzjwi7k,
https://bitbucket.org/azislodowy/spatialmapper﻿ . I started by trying to understand this code, but decided it would just be easier to start over, and grab the parts I needed, and that was mostly mapping a texture to a irregular shaped square, a feature that I really probably did not need.

I have not touched this code in a while, this project is probably not much or use to anyone else, not even sure I would use it again, would maybe consider writing a power point plug-in, if I need something specific. However if there is genuine interest, would be thrilled to work on this again, hopefully collaborate with someone way smarter than me.

What it can do (from what I can remember)
- Remote cueing (forward, backwards, and goto)
- Simple windows phone control app
- some initial OSC support 
- Play Types
	- solid color
	- images
	- videos
	- web cam  (usb)
- Fade between cues

	
**Features that would be cool to add (if I had time and motivation)**

- SPEED/EFFIECENCY (which probably means, not wpf and C#, but rather c++/Open GL)
- wireless webcam support
- Import/Export Show files (as of now much easier to create show on pc itself)
	- would localize resources, change references
	- zip said files
- restore on restart
- return current playing file
