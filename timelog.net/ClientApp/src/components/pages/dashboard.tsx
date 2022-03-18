import React from "react"
import { Stack, Text, ScrollablePane, getTheme, mergeStyleSets, css } from '@fluentui/react';

import { Card } from "../card";
import { TaskList } from "../taskList";
//import Context from "../context";
//import { getDuration, isThisWeek, isToday } from "../../lib/time";
import { IItem } from "./project"
//import { IItem } from "../connectors/entriesConnector";

const theme = getTheme()

const classes = mergeStyleSets({
	fullWidth: {
		position: 'relative',
		width: '75vw',
		margin: '0 auto'
	},
	fullHeight: {
		height: '60vh'
	},
	scrollWrapper: {
		boxShadow: theme.effects.elevation4,
		textAlign: 'center'
	}
})

type ITotal = {
	today: number,
	week: number
}

export const Dashboard = () => {
	const [entries, setEntries] = React.useState<IItem[]>([])
	const [total, setTotal] = React.useState<ITotal>({
		today: 0,
		week: 0
	})

	//const ctx = React.useContext(Context)
	//const { settings, entries: getEntries } = useMemo(() => ctx, [ctx])
	const settings = {
		weeklyHours: 37.5
	}

	/*useEffect(() => {
		(async () => {
			const entries = await getEntries()
			entries.sort((a, b) => b.startTime.getTime() - a.startTime.getTime())

			const messages: { endTime?: Date, category: string, total: number, duration: number }[] = []

			const total: ITotal = { today: 0, week: 0 }
			setEntries(entries.map(e => {
				const duration = getDuration(e.startTime, e.endTime, 'hours')

				if (isToday(e.endTime)) {
					messages.push({ endTime: e.endTime, category: "today", total: total.today, duration: duration })
					total.today += duration
				}

				if (isThisWeek(e.endTime)) {
					messages.push({ endTime: e.endTime, category: "this week", total: total.week, duration: duration })
					total.week += duration
				}

				return {
					...e,
					key: "key-" + e.id,
					duration: duration
				}
			}))
			console.table(messages)

			setTotal(total)
		})()
	}, [getEntries])*/

	return (
		<Stack tokens={{ childrenGap: theme.spacing.m }}>
			<Stack className={classes.fullWidth} horizontal tokens={{ childrenGap: theme.spacing.m }}>
				<Card title="Total Today">
					<Text>{total.today}</Text>
				</Card>
				<Card title="Expected weekly">
					<Text>{settings?.weeklyHours ?? "N/A"}</Text>
				</Card>
				<Card title="Total weekly">
					<Text>{total.week}</Text>
				</Card>
				<Card title="Remaining">
					<Text>{settings ? settings.weeklyHours - total.week : "N/A"}</Text>
				</Card>
			</Stack>
			<div className={css(classes.fullWidth, classes.fullHeight)}>
				<ScrollablePane className={classes.scrollWrapper}>
					<TaskList items={entries} />
				</ScrollablePane>
			</div>
		</Stack>
	)
}