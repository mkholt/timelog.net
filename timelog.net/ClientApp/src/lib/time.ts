import { Text } from "@fluentui/react"
import moment from "moment";
import React from "react";

export const formatTime = (time: Date) => {
	let minutes = (Math.round(time.getMinutes()/15) * 15) % 60;
	let m = moment(time)
	m.set("minutes", minutes)

	let formatted = m.calendar({
		sameDay: "LT"
	})
	return React.createElement(Text, {
		title: formatted,
		children: formatted
	})
}

export const timeSince = (since?: Date) => {
	if (!since) return ""

	let diff = moment().diff(since, "hours", true)
	let duration = moment.duration(diff, "hours")
	console.log(duration)
	return " (" + duration.humanize() + ")"
}

export const getDuration =
	(startTime: moment.MomentInput,
		endTime: moment.MomentInput,
		unit: moment.unitOfTime.Diff = "hours") => moment(endTime).diff(moment(startTime), unit, true)

export const isToday = (time: moment.MomentInput) =>
	moment(time).isBetween(moment().startOf('day'), moment().add(1, 'day').startOf('day'))

export const isThisWeek = (time: moment.MomentInput) =>
	moment(time).isBetween(moment().startOf('week'), moment())
