# BoldTween
Tweening library built to work alongside Unity's open-source UI library.

## Design & Use
The Tweener behaviour is the core around which the rest of BoldTween has been built.

Each Tweener outputs the result of evaluating an AnimationCurve based on the start time and duration of the current tween, while the tween is active. This output can be routed in a number of ways. Unity and native C# callbacks are available, as well as the IInterpolator interface, through which reusable tweening functionalities can be implemented.

Tweeners can tween using a curve and duration defined in the inspector, in a preset asset, or from any other source through various overlaods of the `Tween()` method. They can run using scaled or unscaled time, and a handful of flags can be used to modify how the curve is interpreted:

* `Reverse` evaluates the curve backwards.
* `Invert` subtracts from 1 the evaluated value.
* `Min` iterates over the curve until a value less than the Tweener's current value is found.
* `Max` iterates over the curve until a value greater than the Tweener's current value is found.

When used in conjunction with BoldEvent, a list of EventObject references can be specified to use as tween triggers by a given Tweener.

## Status
BoldTween was used in one of Concordia's submissions to the annual Ubisoft Game Lab challenge. There are no known issues at the moment.

## Installation
Download and unzip into your project.

Works best with:
* [BoldEditor](https://github.com/ophilbinbriscoe/BoldEditor) (nicer inspector)
* [BoldEvent](https://github.com/ophilbinbriscoe/BoldEvent) (extended functionality)

### Projeny
[Projeny](https://github.com/modesttree/Projeny) is an excellent dependency management solution for Unity. While it is not required, all of the ToBoldlyPlay packages were designed with a Projeny workflow in mind.

If you are finding the Projeny documentation a bit daunting, see the following issue for a more detailed guide on getting Projeny up and running: https://github.com/modesttree/Projeny/issues/8.
