import React from 'react';

import {
    CommandBar, ConstrainMode, DetailsList, DetailsListLayoutMode, IColumn, ICommandBarItemProps,
    Link, Text
} from '@fluentui/react';

import { formatTime } from '../lib/time';
import { Actions } from './actions';
//import { AddEntry } from "../addEntry"
//import Context from "../../context"
//import { GetProjects, IItem } from "../../connectors/entriesConnector"
//import { IProject } from "../../../../server/endpoint/projects"
import { toKey } from './menubar';
import { IItem, IProject } from './pages/project';

export type ITaskListProps = {
	items: any[] //IItem[]
}

export const TaskList = ({ items: entries }: ITaskListProps) => {
	//const ctx = React.useContext(Context)
	//const { showDialog } = React.useMemo(() => ctx, [ctx])

	//const [projects, setProjects] = React.useState<IProject[]>([])
	/*React.useEffect(() => {
		GetProjects().then(setProjects)
	}, [])*/
	
	const projects = React.useMemo(() => [], [])

	const columns = React.useMemo(() => buildColumns(projects), [projects])

	const commandItems: ICommandBarItemProps[] = [
		{
			key: "command_add",
			text: "Add",
			iconProps: { iconName: "add" },
			/*onClick: () => showDialog && showDialog({
				title: "Add entry",
				content: <AddEntry />
			}),*/
			disabled: true// !showDialog
		}
	]

	return (
		<>
			<CommandBar items={commandItems} />
			{entries?.length ? (
				<DetailsList
					items={entries}
					columns={columns}
					layoutMode={DetailsListLayoutMode.justified}
					constrainMode={ConstrainMode.unconstrained}
				/>
			) : (
				 <Text variant="large">No entries found</Text>
			 )}
		</>
	)
}

const buildColumn = (name: string, minWidth: number, maxWidth?: number, onRender?: (e: IItem) => JSX.Element|string, fieldName?: string, isSortedDescending?: boolean, hideName?: boolean): IColumn => (
	{
		key: toKey(name, "column"),
		name: hideName ? "" : name,
		minWidth: minWidth,
		maxWidth: maxWidth,
		onRender: onRender,
		fieldName: fieldName,
		isSorted: isSortedDescending !== undefined ? true : undefined,
		isSortedDescending: isSortedDescending
	}
)

const buildColumns = (projects: IProject[]) => [
	buildColumn('Project', 100, 300, (e: IItem) => (
			<Link href={"/projects/" + e.task.projectId}>
				{projects.find(p => p.projectId === e.task.projectId)?.title ?? "N/A"}
			</Link>)
	),
	buildColumn('Task', 200, undefined, (e: IItem) => e.task.taskId + " - " + e.task.title),
	buildColumn('Start', 150, undefined, (e: IItem) => formatTime(e.startTime), undefined, true),
	buildColumn('End', 150, undefined, (e: IItem) => e.endTime ? formatTime(e.endTime) : ""),
	buildColumn('Hours', 100, undefined, (e: IItem) => e.duration.toString()),
	buildColumn('Actions', 100, undefined, (e: IItem) => <Actions item={e} />, undefined, undefined, true)
]