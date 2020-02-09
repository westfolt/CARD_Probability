# CARD_Probability
 Program for RADAR detection probability calculation

Program reads ASTERIX format frames from files on local disk, that have
been written by third-party soft. Decodes 34 and 48 categories, selects cat48 packets, and calculates detection probability.\

06.01
Added fields for PSR probability calculation and additional info that
helps operator to estimate information usability (probability calculated from low number of scans is not useful)

17.01
Added PSR Probability calculation,based on aircrafts already detected by SSR, in PSR sweep area
TODO:
add PR calculation for PSR
add scale adjustments

29.01
Additional scales added, not used yet

01.02
Scales are working now properly
Near radar zone scale added

02.02
Minor bug fixes
Added ability to choose source for color on screen: PSR or SSR detection probability
Ability to work with files currently in use

09.02
Help added, PR coefficients to display changed