import React from 'react';

import FluentIcons from '@fluentui/font-icons-mdl2/lib/data/AllIconNames.json';
import { getTheme, Icon, ISelectableOption, Stack, VirtualizedComboBox } from '@fluentui/react';

export type IIconPickerProps = {
	label: string
	value?: string
	onChange: (i: string) => void
}

const theme = getTheme()

export const IconPicker = (props: IIconPickerProps) => {
	const [ selected, setSelected ] = React.useState<string>()
	
	const iconOptions = React.useMemo(() => getOptions(), [])
	React.useEffect(() => setSelected(props.value), [props.value])

	const onRenderOption = (item: ISelectableOption|undefined) => {
		if (!item) return null
		return (
			<div>
				<Icon iconName={item.key as string} />{' '}
				<span>{item.text}</span>
			</div>
		)
	}
	
	return (
		<Stack horizontal tokens={{ childrenGap: theme.spacing.s2 }} verticalAlign={"end"}>
			<VirtualizedComboBox
				options={iconOptions}
				selectedKey={selected}
				onChange={(_, s) => setSelected(s?.key as string)}
				autoComplete={"on"}
				label={props.label}
				onRenderOption={onRenderOption}
			/>
			<Icon iconName={selected} />
		</Stack>
	)
}
const normalize = (s: string): string => s.replace(/([A-Z])/g, " $1")

type NamedIcon = {name: string, unicode: string}
const getOptions = () => FluentIcons.filter((i): i is NamedIcon => !!i.name).sort((a, b) => a.name > b.name ? 1 : (b.name > a.name ? -1 : 0))
	.map(i => ({
		key: i.name,
		text: normalize(i.name)
	}))
